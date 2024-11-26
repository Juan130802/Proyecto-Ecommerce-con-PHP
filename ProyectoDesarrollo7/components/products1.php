<?php

if (!isset($_SESSION['cart'])) {
    $_SESSION['cart'] = [];
}

if (isset($_POST['action']) && $_POST['action'] === 'add') {
    if (!isset($_SESSION['usuario_id'])) {
        echo "<script>alert('Debes iniciar sesión para agregar productos al carrito.');</script>";
        header("Location: login.php");
        exit;
    }
}

if (isset($_POST['action']) && $_POST['action'] === 'add') {
    $productName = $_POST['name'];
    $productPrice = $_POST['price'];
    $productImage = $_POST['image'];

    $productId = md5($productName);

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

    header("Location: " . $_SERVER['PHP_SELF']);
    exit;
}

$apiUrl = 'https://fakestoreapi.com/products';
$response = file_get_contents($apiUrl);
$products = json_decode($response, true);

$electronics = array_filter($products, function($product) {
    $techCategories = ['electronics'];
    return in_array($product['category'], $techCategories);
});
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Tienda de Productos Tecnológicos</title>
    <script src="https://cdn.tailwindcss.com"></script>
</head>
<body class="bg-gray-50">

<div class="container mx-auto px-4 py-16">
    <h2 class="text-3xl font-bold mb-8 text-center">Productos Tecnológicos Disponibles</h2>
    <div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        <?php foreach ($electronics as $product): ?>
            <div class="bg-white rounded-xl shadow-md overflow-hidden hover:shadow-lg transition duration-300">
                <img src="<?php echo htmlspecialchars($product['image']); ?>" alt="<?php echo htmlspecialchars($product['title']); ?>" class="w-full h-48 object-cover">
                <div class="p-6">
                    <h3 class="text-xl font-semibold mb-2"><?php echo htmlspecialchars($product['title']); ?></h3>
                    <p class="text-gray-600 mb-4"><?php echo htmlspecialchars($product['description']); ?></p>
                    <div class="flex justify-between items-center">
                        <span class="text-2xl font-bold text-indigo-600">$<?php echo htmlspecialchars($product['price']); ?></span>
                        <form method="POST">
                            <input type="hidden" name="action" value="add">
                            <input type="hidden" name="name" value="<?php echo htmlspecialchars($product['title']); ?>">
                            <input type="hidden" name="price" value="<?php echo htmlspecialchars($product['price']); ?>">
                            <input type="hidden" name="image" value="<?php echo htmlspecialchars($product['image']); ?>">
                            <button type="submit" class="bg-indigo-600 text-white px-4 py-2 rounded-lg hover:bg-indigo-700 transition duration-300">
                                Añadir al carrito
                            </button>
                        </form>
                    </div>
                </div>
            </div>
        <?php endforeach; ?>
    </div>
</div>
</body>
</html>
