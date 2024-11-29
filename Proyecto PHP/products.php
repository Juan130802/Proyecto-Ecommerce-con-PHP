<?php
session_start();

if (!isset($_SESSION['cart'])) {
    $_SESSION['cart'] = [];
}

if (isset($_POST['action']) && $_POST['action'] === 'add') {
    if (!isset($_SESSION['usuario_id'])) {
        echo "<script>alert('Debes iniciar sesión para agregar productos al carrito.');</script>";
        header("Location: login.php");
        exit;
    }

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

$filteredProducts = $products;

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $minPrice = isset($_POST['minPrice']) && $_POST['minPrice'] !== '' ? floatval($_POST['minPrice']) : null;
    $maxPrice = isset($_POST['maxPrice']) && $_POST['maxPrice'] !== '' ? floatval($_POST['maxPrice']) : null;

    $selectedCategories = $_POST['categories'] ?? [];

    $filteredProducts = array_filter($products, function($product) use ($minPrice, $maxPrice, $selectedCategories) {
        $inPriceRange = true;
        $inCategory = true;

        if ($minPrice !== null) {
            $inPriceRange = $inPriceRange && $product['price'] >= $minPrice;
        }
        if ($maxPrice !== null) {
            $inPriceRange = $inPriceRange && $product['price'] <= $maxPrice;
        }

        if (!empty($selectedCategories)) {
            $inCategory = in_array($product['category'], $selectedCategories);
        }

        return $inPriceRange && $inCategory;
    });
}

?>

<?php include 'components/header.php'; ?>
<?php include 'components/navbar.php'; ?>

<div class="container mx-auto px-4 py-8">
    <div class="flex flex-col md:flex-row gap-8">
        <div class="w-full md:w-64 bg-white p-6 rounded-lg shadow-md h-fit">
            <h3 class="text-lg font-semibold mb-4">Filtros</h3>
            <form method="POST" action="">
                <div class="mb-6">
                    <h4 class="font-medium mb-2">Rango de Precio</h4>
                    <input type="number" name="minPrice" class="border p-2 w-full" placeholder="Precio Mínimo">
                    <input type="number" name="maxPrice" class="border p-2 w-full mt-2" placeholder="Precio Máximo">
                </div>

                <div class="mb-6">
                    <h4 class="font-medium mb-2">Categorías</h4>
                    <?php
                    $categories = array_unique(array_column($products, 'category'));
                    foreach ($categories as $category): ?>
                        <label class="flex items-center">
                            <input type="checkbox" name="categories[]" value="<?php echo $category; ?>" class="form-checkbox text-indigo-600">
                            <span class="ml-2"><?php echo ucfirst($category); ?></span>
                        </label>
                    <?php endforeach; ?>
                </div>

                <button type="submit" class="w-full bg-indigo-600 text-white py-2 rounded-lg hover:bg-indigo-700 transition duration-300">
                    Aplicar Filtros
                </button>
            </form>
        </div>

        <div class="flex-1">
            <h2 class="text-2xl font-bold mb-6">Productos Filtrados</h2>
            <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                <?php foreach ($filteredProducts as $product): ?>
                    <div class="bg-white rounded-xl shadow-md overflow-hidden hover:shadow-lg transition duration-300 product-card">
                        <img src="<?php echo $product['image']; ?>" alt="<?php echo $product['title']; ?>" class="w-full h-48 object-cover">
                        <div class="p-6">
                            <h3 class="text-xl font-semibold mb-2"><?php echo $product['title']; ?></h3>
                            <p class="text-gray-600 mb-4"><?php echo $product['description']; ?></p>
                            <div class="flex justify-between items-center">
                                <span class="text-2xl font-bold text-indigo-600">$<?php echo $product['price']; ?></span>
                                <form method="POST" action="">
                                    <input type="hidden" name="action" value="add">
                                    <input type="hidden" name="name" value="<?php echo $product['title']; ?>">
                                    <input type="hidden" name="price" value="<?php echo $product['price']; ?>">
                                    <input type="hidden" name="image" value="<?php echo $product['image']; ?>">
                                    <button class="bg-indigo-600 text-white px-4 py-2 rounded-lg hover:bg-indigo-700 transition duration-300">
                                        Añadir al carrito
                                    </button>
                                </form>
                            </div>
                        </div>
                    </div>
                <?php endforeach; ?>
            </div>
        </div>
    </div>
</div>

<?php include 'components/footer.php'; ?>
