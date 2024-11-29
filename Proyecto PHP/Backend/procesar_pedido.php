<?php
session_start();
require 'conexionBD.php';

if (!isset($_SESSION['usuario_id'])) {
    renderError("No tienes permiso para acceder a esta página.");
    exit;
}

$userId = $_SESSION['usuario_id'];
$total = $_POST['total'] ?? null;
$pais = $_POST['pais'] ?? null;
$cart = $_SESSION['cart'] ?? null;

if (empty($cart)) {
    renderError("El carrito está vacío.");
    exit;
}

if (empty($pais)) {
    renderError("Debes seleccionar un país de destino.");
    exit;
}

$mysqli = conectarDB();
$mysqli->begin_transaction();

try {
    $stmtPedido = $mysqli->prepare("INSERT INTO pedidos (idUsuario, total, pais, estado) VALUES (?, ?, ?, 'pendiente')");
    $stmtPedido->bind_param("ids", $userId, $total, $pais);
    $stmtPedido->execute();
    $idPedido = $mysqli->insert_id;
    $stmtPedido->close();

    $stmtDetalle = $mysqli->prepare("INSERT INTO detallepedidos (idPedido, idProducto, cantidad, precioUnitario, subtotal, pais) VALUES (?, ?, ?, ?, ?, ?)");
    foreach ($cart as $idProducto => $item) {
        $cantidad = $item['quantity'];
        $precioUnitario = $item['price'];
        $subtotal = $precioUnitario * $cantidad;
        $stmtDetalle->bind_param("iiidds", $idPedido, $idProducto, $cantidad, $precioUnitario, $subtotal, $pais);
        $stmtDetalle->execute();
    }
    $stmtDetalle->close();

    actualizarEstadoPedidos($mysqli);

    $mysqli->commit();

    $_SESSION['cart'] = [];
    setcookie("cart_user_" . $userId, "", time() - 3600, "/");
} catch (Exception $e) {
    $mysqli->rollback();
    renderError("Error al realizar el pedido: " . $e->getMessage());
    exit;
}

$mysqli->close();

function actualizarEstadoPedidos($mysqli)
{
    $query = "SELECT idPedido, estado FROM pedidos WHERE estado IN ('pendiente', 'confirmado', 'no confirmado')";
    $result = $mysqli->query($query);

    if (!$result) {
        die("Error en la consulta SELECT: " . $mysqli->error);
    }

    if ($result->num_rows > 0) {
        while ($pedido = $result->fetch_assoc()) {
            $idPedido = $pedido['idPedido'];
            $estadoActual = $pedido['estado'];

            $nuevoEstado = '';
            switch ($estadoActual) {
                case 'pendiente':
                    $nuevoEstado = (rand(0, 1) == 0) ? 'confirmado' : 'no confirmado';
                    break;
                case 'confirmado':
                    $nuevoEstado = 'entregado';
                    break;
                case 'no confirmado':
                    $nuevoEstado = 'no entregado';
                    break;
            }

            if (!empty($nuevoEstado)) {
                $stmt = $mysqli->prepare("UPDATE pedidos SET estado = ? WHERE idPedido = ?");
                $stmt->bind_param("si", $nuevoEstado, $idPedido);
                $stmt->execute();
                $stmt->close();
            }
        }
    }
}


function renderError($message)
{
    echo <<<HTML
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Error</title>
    <script src="https://cdn.tailwindcss.com"></script>
</head>
<body class="bg-gray-50">
    <div class="min-h-screen flex items-center justify-center">
        <div class="bg-red-50 border border-red-400 text-red-700 rounded-lg p-6 max-w-md w-full">
            <h1 class="text-2xl font-semibold mb-4">Ocurrió un Error</h1>
            <p class="mb-4">$message</p>
            <a href="javascript:history.back()" class="block bg-red-600 hover:bg-red-700 text-white py-2 px-4 rounded-lg text-lg text-center">
                Volver
            </a>
        </div>
    </div>
</body>
</html>
HTML;
    exit;
}
?>

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Confirmación de Pedido</title>
    <script src="https://cdn.tailwindcss.com"></script>
</head>

<body class="bg-gray-50">
    <div class="min-h-screen flex items-center justify-center">
        <div class="bg-white shadow-md rounded-lg p-8 max-w-md w-full text-center">
            <div class="mb-6">
                <h1 class="text-2xl font-semibold text-gray-800">¡Pedido Realizado Exitosamente!</h1>
                <p class="text-gray-600 mt-2">Gracias por tu compra. Tu pedido ha sido procesado correctamente.</p>
            </div>
            <div class="bg-gray-100 rounded-lg p-4 mb-6">
                <p class="text-gray-700 text-lg font-medium">ID del Pedido: <span
                        class="font-bold text-indigo-600"><?php echo $idPedido; ?></span></p>
                <p class="text-gray-700 text-lg font-medium">Total: <span
                        class="font-bold text-indigo-600">$<?php echo number_format($total, 2); ?></span></p>
            </div>
            <a href="../ver_pedidos.php"
                class="block bg-indigo-600 hover:bg-indigo-700 text-white py-2 px-4 rounded-lg text-lg">
                Ver Mis Pedidos
            </a>
        </div>
    </div>
</body>

</html>