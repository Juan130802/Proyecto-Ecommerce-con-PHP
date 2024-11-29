<?php
if (session_status() == PHP_SESSION_NONE) {
    session_start();
}

$totalItems = 0;
if (isset($_SESSION['cart'])) {
    foreach ($_SESSION['cart'] as $item) {
        $totalItems += $item['quantity'];
    }
}

if (isset($_GET['logout']) && $_GET['logout'] == 'true') {
    session_unset();
    session_destroy();
    header("Location: index.php");
    exit();
}
?>
<nav class="bg-indigo-600 text-white shadow-lg"> 
    <div class="container mx-auto px-4 py-3">
        <div class="flex justify-between items-center">
            <div class="flex items-center space-x-4">
                <h1 class="text-2xl font-bold">TechStore</h1>
                <div class="hidden md:flex space-x-4">
                    <a href="index.php" class="hover:text-indigo-200">Inicio</a>
                    <a href="products.php" class="hover:text-indigo-200">Productos</a>
                </div>
            </div>
            <div class="flex items-center space-x-4">
                <a href="cart.php" class="relative hover:text-indigo-200">
                    <?php if ($totalItems > 0): ?>
                        <span class="absolute -top-3 right-0 bg-red-500 text-white text-xs font-bold rounded-full w-5 h-5 flex items-center justify-center transform -translate-x-1/2">
                            <?php echo $totalItems; ?>
                        </span>
                    <?php endif; ?>
                    
                    <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
                    </svg>
                </a>

                <a href="cuenta.php" class="flex items-center space-x-2 hover:text-indigo-200">
                    <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5.121 17.804A4 4 0 015 16c0-2.21 2.015-4 4.5-4S14 13.79 14 16c0 .727-.195 1.4-.536 1.996m-7.707-2.792a3.993 3.993 0 011.243-1.215M7.879 17.804A4 4 0 018 16c0-2.21 2.015-4 4.5-4S17 13.79 17 16a4 4 0 01-.5 1.804M8.5 7A1.5 1.5 0 1011 8.5 1.5 1.5 0 008.5 7zM12 10.5A1.5 1.5 0 1110.5 9 1.5 1.5 0 0112 10.5zM15 7A1.5 1.5 0 1016.5 8.5 1.5 1.5 0 0015 7z" />
                    </svg>
                    <span>Cuenta</span>
                </a>
                
                <?php if (isset($_SESSION['usuario_id'])): ?>
                    <form action="Backend/logout.php" method="GET" style="display:inline;">
                        <button type="submit" class="bg-white text-indigo-600 px-4 py-2 rounded-lg hover:bg-indigo-100">Cerrar Sesión</button>
                    </form>
                <?php else: ?>
                    <a href="login.php" class="hover:text-indigo-200">Iniciar Sesión</a>
                    <a href="register.php" class="bg-white text-indigo-600 px-4 py-2 rounded-lg hover:bg-indigo-100">Registrarse</a>
                <?php endif; ?>
            </div>
        </div>
    </div>
</nav>

