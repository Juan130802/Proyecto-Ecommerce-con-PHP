<?php
session_start();
require 'Backend/conexionBD.php';

if (!isset($_SESSION['usuario_id'])) {
    die("No tienes permiso para acceder a esta página.");
}

$userId = $_SESSION['usuario_id'];
$mysqli = conectarDB();

// Número de pedidos por página
$limite = 10;
$pagina = isset($_GET['pagina']) ? (int)$_GET['pagina'] : 1;
$inicio = ($pagina - 1) * $limite;

// Obtener los pedidos del usuario con paginación
$queryPedidos = "SELECT idPedido, fechaPedido, total, estado FROM pedidos WHERE idUsuario = ? LIMIT ?, ?";
$stmtPedidos = $mysqli->prepare($queryPedidos);
$stmtPedidos->bind_param("iii", $userId, $inicio, $limite);
$stmtPedidos->execute();
$resultPedidos = $stmtPedidos->get_result();

// Calcular el total de páginas
$queryCount = "SELECT COUNT(*) as total FROM pedidos WHERE idUsuario = ?";
$stmtCount = $mysqli->prepare($queryCount);
$stmtCount->bind_param("i", $userId);
$stmtCount->execute();
$totalPedidos = $stmtCount->get_result()->fetch_assoc()['total'];
$totalPaginas = ceil($totalPedidos / $limite);

// Función para traducir estados
function traducirEstado($estado) {
    $estados = [
        'pendiente' => 'Pendiente de Confirmación',
        'procesado' => 'En Proceso',
        'entregado' => 'Entregado',
    ];
    return $estados[$estado] ?? 'Desconocido';
}
?>

<?php include 'components/header.php'; ?>
<?php include 'components/navbar.php'; ?>

<div class="min-h-screen bg-gray-50 py-12">
    <div class="container mx-auto px-4">
        <h1 class="text-3xl font-bold mb-8">Mis Pedidos</h1>
        <?php if ($resultPedidos->num_rows > 0): ?>
            <div class="bg-white rounded-lg shadow-md p-6">
                <table class="w-full border-collapse border">
                    <thead>
                        <tr class="bg-gray-200">
                            <th class="border p-2">ID Pedido</th>
                            <th class="border p-2">Fecha</th>
                            <th class="border p-2">Total</th>
                            <th class="border p-2">Estado</th>
                            <th class="border p-2">Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
                        <?php while ($pedido = $resultPedidos->fetch_assoc()): ?>
                            <tr>
                                <td class="border p-2"><?php echo $pedido['idPedido']; ?></td>
                                <td class="border p-2"><?php echo date('d/m/Y', strtotime($pedido['fechaPedido'])); ?></td>
                                <td class="border p-2">$<?php echo number_format($pedido['total'], 2); ?></td>
                                <td class="border p-2"><?php echo traducirEstado($pedido['estado']); ?></td>
                                <td class="border p-2">
                                    <a href="verDetalle.php?id=<?php echo $pedido['idPedido']; ?>" class="text-blue-500 hover:underline">Ver Detalle</a>
                                </td>
                            </tr>
                        <?php endwhile; ?>
                    </tbody>
                </table>
            </div>

            <!-- Paginación -->
            <div class="mt-6 flex justify-center">
                <?php for ($i = 1; $i <= $totalPaginas; $i++): ?>
                    <a href="?pagina=<?php echo $i; ?>" class="px-3 py-1 mx-1 border <?php echo $i == $pagina ? 'bg-blue-500 text-white' : 'bg-gray-200'; ?>">
                        <?php echo $i; ?>
                    </a>
                <?php endfor; ?>
            </div>
        <?php else: ?>
            <p class="text-gray-600">No tienes pedidos realizados.</p>
        <?php endif; ?>
    </div>
</div>
