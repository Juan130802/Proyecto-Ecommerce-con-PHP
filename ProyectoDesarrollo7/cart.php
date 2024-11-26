<?php
session_start();

if (!isset($_SESSION['usuario_id'])) {
    die("No tienes permiso para acceder a esta página.");
}

$userId = $_SESSION['usuario_id'];
$cartCookieName = "cart_user_" . $userId;

if (!isset($_SESSION['cart'])) {
    if (isset($_COOKIE[$cartCookieName])) {
        $_SESSION['cart'] = json_decode($_COOKIE[$cartCookieName], true);
    } else {
        $_SESSION['cart'] = [];
    }
}

function updateCartCookie($userId) {
    global $cartCookieName;
    setcookie($cartCookieName, json_encode($_SESSION['cart']), time() + (86400 * 30), "/"); // Validez de 30 días
}

if (isset($_POST['action'])) {
    $action = $_POST['action'];
    $productId = $_POST['id'] ?? null;

    switch ($action) {
        case 'add':
            $productName = $_POST['name'];
            $productPrice = $_POST['price'];
            $productImage = $_POST['image'];

            if (isset($_SESSION['cart'][$productId])) {
                $_SESSION['cart'][$productId]['quantity']++;
            } else {
                $_SESSION['cart'][$productId] = [
                    'name' => $productName,
                    'price' => $productPrice,
                    'image' => $productImage,
                    'quantity' => 1
                ];
            }
            break;

        case 'update':
            $quantity = $_POST['quantity'];
            if (isset($_SESSION['cart'][$productId])) {
                if ($quantity > 0) {
                    $_SESSION['cart'][$productId]['quantity'] = $quantity;
                } else {
                    unset($_SESSION['cart'][$productId]);
                }
            }
            break;

        case 'remove':
            unset($_SESSION['cart'][$productId]);
            break;

        case 'clear':
            $_SESSION['cart'] = [];
            break;
    }

    updateCartCookie($userId);
}

function calculateTotal()
{
    $subtotal = 0;
    foreach ($_SESSION['cart'] as $item) {
        $subtotal += $item['price'] * $item['quantity'];
    }
    $tax = $subtotal * 0.15;
    $total = $subtotal + $tax;

    return [
        'subtotal' => $subtotal,
        'tax' => $tax,
        'total' => $total
    ];
}

$totals = calculateTotal();
?>



<?php include 'components/header.php'; ?>
<?php include 'components/navbar.php'; ?>

<div class="min-h-screen bg-gray-50 py-12">
    <div class="container mx-auto px-4">
        <h1 class="text-3xl font-bold mb-8">Carrito de Compras</h1>

        <div class="flex flex-col lg:flex-row gap-8">
            <div class="flex-grow">
                <?php if (!empty($_SESSION['cart'])): ?>
                    <div class="bg-white rounded-lg shadow-md p-6 mb-4">
                        <div class="flex items-center justify-between border-b pb-4">
                            <h2 class="text-xl font-semibold">Productos</h2>
                            <span class="text-gray-600"><?php echo count($_SESSION['cart']); ?> items</span>
                        </div>

                        <?php foreach ($_SESSION['cart'] as $id => $item): ?>
                            <div class="py-4 border-b">
                                <div class="flex items-center gap-4">
                                    <img src="<?php echo $item['image']; ?>" 
                                         alt="<?php echo $item['name']; ?>" 
                                         class="w-24 h-24 object-cover rounded-lg">
                                    <div class="flex-grow">
                                        <h3 class="text-lg font-semibold"><?php echo $item['name']; ?></h3>
                                        <div class="flex items-center gap-4 mt-2">
                                            <form method="POST" class="flex items-center">
                                                <input type="hidden" name="action" value="update">
                                                <input type="hidden" name="id" value="<?php echo $id; ?>">
                                                <div class="flex items-center border rounded-lg">
                                                    <button type="submit" name="quantity" value="<?php echo $item['quantity'] - 1; ?>" class="px-3 py-1 hover:bg-gray-100 text-gray-600">-</button>
                                                    <span class="px-3 py-1 border-x"><?php echo $item['quantity']; ?></span>
                                                    <button type="submit" name="quantity" value="<?php echo $item['quantity'] + 1; ?>" class="px-3 py-1 hover:bg-gray-100 text-gray-600">+</button>
                                                </div>
                                            </form>
                                            <form method="POST">
                                                <input type="hidden" name="action" value="remove">
                                                <input type="hidden" name="id" value="<?php echo $id; ?>">
                                                <button type="submit" class="text-red-500 hover:text-red-700">
                                                    Eliminar
                                                </button>
                                            </form>
                                        </div>
                                    </div>
                                    <div class="text-right">
                                        <p class="text-lg font-bold text-indigo-600">$<?php echo number_format($item['price'] * $item['quantity'], 2); ?></p>
                                        <p class="text-sm text-gray-500">$<?php echo number_format($item['price'], 2); ?> cada uno</p>
                                    </div>
                                </div>
                            </div>
                        <?php endforeach; ?>
                    </div>
                    <div class="flex justify-between items-center">
                        <a href="products.php" class="text-indigo-600 hover:text-indigo-700">Continuar Comprando</a>
                        <a href="ver_pedidos.php" class="text-blue-500 hover:text-blue-700">Ver Mis Pedidos</a>
                        <form method="POST">
                            <input type="hidden" name="action" value="clear">
                            <button type="submit" class="text-red-500 hover:text-red-700">Vaciar Carrito</button>
                        </form>
                    </div>
                <?php else: ?>
                    <p class="text-gray-600">El carrito está vacío.</p>
                <?php endif; ?>
            </div>

            <div class="lg:w-96">
                <div class="bg-white rounded-lg shadow-md p-6">
                    <h2 class="text-xl font-semibold mb-4">Resumen del Pedido</h2>
                    <div class="space-y-3 text-gray-600">
                        <div class="flex justify-between">
                            <span>Subtotal</span>
                            <span>$<?php echo number_format($totals['subtotal'], 2); ?></span>
                        </div>
                        <div class="flex justify-between">
                            <span>Impuestos</span>
                            <span>$<?php echo number_format($totals['tax'], 2); ?></span>
                        </div>
                    </div>
                    <div class="border-t mt-4 pt-4">
                        <div class="flex justify-between items-center font-semibold text-lg">
                            <span>Total</span>
                            <span class="text-indigo-600">$<?php echo number_format($totals['total'], 2); ?></span>
                        </div>
                    </div>
                    <form method="POST" action="Backend/procesar_pedido.php">
                        <input type="hidden" name="total" value="<?php echo $totals['total']; ?>">
                        <button type="submit" class="bg-indigo-600 text-white px-6 py-2 rounded-lg hover:bg-indigo-700">Hacer Pedido</button>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>
