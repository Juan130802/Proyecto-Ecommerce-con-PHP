<?php
session_start();
require 'Backend/conexionBD.php';

if (!isset($_SESSION['usuario_id'])) {
    die("No tienes permiso para acceder a esta página.");
}

if (!isset($_GET['id'])) {
    die("Pedido no especificado.");
}

$idPedido = (int) $_GET['id'];
$mysqli = conectarDB();

$queryDetalle = "
    SELECT dp.cantidad, dp.precioUnitario, dp.subtotal, p.pais
    FROM detallepedidos dp
    INNER JOIN pedidos p ON dp.idPedido = p.idPedido
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
        <!-- Encabezado con botón de Volver -->
        <div class="flex justify-between items-center mb-8">
            <h1 class="text-3xl font-bold">Detalle del Pedido</h1>
            <a href="javascript:history.back()"
                class="bg-red-600 hover:bg-red-700 text-white py-2 px-4 rounded-lg text-lg">
                Volver
            </a>
        </div>

        <!-- Tabla de detalles -->
        <?php if ($resultDetalle->num_rows > 0): ?>
            <div class="bg-white rounded-lg shadow-md p-6">
                <table class="w-full border-collapse border">
                    <thead>
                        <tr class="bg-gray-200">
                            <th class="border p-2">Cantidad</th>
                            <th class="border p-2">Precio Unitario</th>
                            <th class="border p-2">Subtotal</th>
                            <th class="border p-2">País</th>
                        </tr>
                    </thead>
                    <tbody>
                        <?php while ($detalle = $resultDetalle->fetch_assoc()): ?>
                            <tr>
                                <td class="border p-2"><?php echo $detalle['cantidad']; ?></td>
                                <td class="border p-2">$<?php echo number_format($detalle['precioUnitario'], 2); ?></td>
                                <td class="border p-2">$<?php echo number_format($detalle['subtotal'], 2); ?></td>
                                <td class="border p-2"><?php echo htmlspecialchars($detalle['pais']); ?></td>
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