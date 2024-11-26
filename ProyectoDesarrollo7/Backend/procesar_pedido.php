<?php
session_start();
require 'conexionBD.php';

if (!isset($_SESSION['usuario_id'])) {
    die("No tienes permiso para acceder a esta página.");
}

$userId = $_SESSION['usuario_id'];
$total = $_POST['total'];
$cart = $_SESSION['cart'];

if (empty($cart)) {
    echo "
    <html lang='es'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Carrito Vacío</title>
        <script src='https://cdn.tailwindcss.com'></script>
    </head>
    <body class='bg-gray-50'>
        <div class='min-h-screen flex items-center justify-center'>
            <div class='bg-white shadow-md rounded-lg p-8 max-w-md w-full text-center'>
                <div class='mb-6'>
                    <h1 class='text-2xl font-semibold text-gray-800'>¡Tu carrito está vacío!</h1>
                    <p class='text-gray-600 mt-2'>No has agregado productos al carrito. ¿Qué te gustaría hacer?</p>
                </div>
                <div class='flex justify-center gap-4'>
                    <a href='../cart.php' class='bg-indigo-600 hover:bg-indigo-700 text-white py-2 px-4 rounded-lg text-lg'>
                        Volver a la tienda
                    </a>
                    <a href='../index.php' class='bg-gray-300 hover:bg-gray-400 text-gray-800 py-2 px-4 rounded-lg text-lg'>
                        Volver al inicio
                    </a>
                </div>
            </div>
        </div>
    </body>
    </html>
    ";
    exit;
}

$mysqli = conectarDB();
$mysqli->begin_transaction();

try {
    $stmtPedido = $mysqli->prepare("INSERT INTO pedidos (idUsuario, total) VALUES (?, ?)");
    $stmtPedido->bind_param("id", $userId, $total);
    $stmtPedido->execute();
    $idPedido = $mysqli->insert_id;
    $stmtPedido->close();

    $stmtDetalle = $mysqli->prepare("INSERT INTO detallepedidos (idPedido, idProducto, cantidad, precioUnitario, subtotal) VALUES (?, ?, ?, ?, ?)");

    foreach ($cart as $idProducto => $item) {
        $cantidad = $item['quantity'];
        $precioUnitario = $item['price'];
        $subtotal = $precioUnitario * $cantidad;

        $stmtDetalle->bind_param("iiidd", $idPedido, $idProducto, $cantidad, $precioUnitario, $subtotal);
        $stmtDetalle->execute();
    }

    $stmtDetalle->close();
    $mysqli->commit();

    $_SESSION['cart'] = [];
    setcookie("cart_user_" . $userId, "", time() - 3600, "/");
} catch (Exception $e) {
    $mysqli->rollback();
    echo "Error al realizar el pedido: " . $e->getMessage();
    $mysqli->close();
    exit;
}

$mysqli->close();
?>

<!DOCTYPE html>
<html lang="es">
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
                <p class="text-gray-700 text-lg font-medium">ID del Pedido: <span class="font-bold text-indigo-600"><?php echo $idPedido; ?></span></p>
                <p class="text-gray-700 text-lg font-medium">Total: <span class="font-bold text-indigo-600">$<?php echo number_format($total, 2); ?></span></p>
            </div>
            <a href="../ver_pedidos.php" class="block bg-indigo-600 hover:bg-indigo-700 text-white py-2 px-4 rounded-lg text-lg">
                Ver Mis Pedidos
            </a>
        </div>
    </div>
</body>
</html>
