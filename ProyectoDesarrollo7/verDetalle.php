<?php
session_start();
require 'Backend/conexionBD.php';

if (!isset($_SESSION['usuario_id'])) {
    die("No tienes permiso para acceder a esta página.");
}

if (!isset($_GET['id'])) {
    die("Pedido no especificado.");
}

$idPedido = (int)$_GET['id'];
$mysqli = conectarDB();

// Consulta para obtener los detalles del pedido
$queryDetalle = "SELECT dp.cantidad, dp.precioUnitario, dp.subtotal 
                 FROM detallepedidos dp
                 WHERE dp.idPedido = ?";
$stmtDetalle = $mysqli->prepare($queryDetalle);
$stmtDetalle->bind_param("i", $idPedido);
$stmtDetalle->execute();
$resultDetalle = $stmtDetalle->get_result();

?>
<?php include 'components/header.php'; ?>
<?php include 'components/navbar.php'; ?>

<div class="min-h-screen bg-gray-50 py-12">
    <div class="container mx-auto px-4">
        <h1 class="text-3xl font-bold mb-8">Detalle del Pedido</h1>
        <?php if ($resultDetalle->num_rows > 0): ?>
            <div class="bg-white rounded-lg shadow-md p-6">
                <table class="w-full border-collapse border">
                    <thead>
                        <tr class="bg-gray-200">
                            <th class="border p-2">Cantidad</th>
                            <th class="border p-2">Precio Unitario</th>
                            <th class="border p-2">Subtotal</th>
                        </tr>
                    </thead>
                    <tbody>
                        <?php while ($detalle = $resultDetalle->fetch_assoc()): ?>
                            <tr>
                                <td class="border p-2"><?php echo $detalle['cantidad']; ?></td>
                                <td class="border p-2">$<?php echo number_format($detalle['precioUnitario'], 2); ?></td>
                                <td class="border p-2">$<?php echo number_format($detalle['subtotal'], 2); ?></td>
                            </tr>
                        <?php endwhile; ?>
                    </tbody>
                </table>
            </div>
        <?php else: ?>
            <p class="text-gray-600">No hay detalles disponibles para este pedido.</p>
        <?php endif; ?>
    </div>
</div>
