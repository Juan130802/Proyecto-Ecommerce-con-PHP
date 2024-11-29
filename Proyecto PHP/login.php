<?php
session_start();

if (isset($_COOKIE['cart'])) {
    $_SESSION['cart'] = json_decode($_COOKIE['cart'], true);
    setcookie('cart', '', time() - 3600, "/");
}

include 'Backend/conexionBD.php';
$mysqli = conectarDB();

if (isset($_SESSION['usuario_id'])) {
    header("Location: index.php");
    exit();
}

$error_message = ''; 

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $correo = $_POST['correo'] ?? '';
    $contrasena = $_POST['contrasena'] ?? '';

    if (!empty($correo) && !empty($contrasena)) {
        $sql = "SELECT * FROM usuarios WHERE correo = ?";
        $stmt = $mysqli->prepare($sql);
        $stmt->bind_param("s", $correo);
        $stmt->execute();
        $result = $stmt->get_result();

        if ($result->num_rows > 0) {
            $row = $result->fetch_assoc();

            if ($row['estado'] === 'inactivo') {
                $error_message = 'Tu cuenta ha sido desactivada. Contacta al soporte para más información.';
            } elseif (password_verify($contrasena, $row['contrasena'])) {
                $_SESSION['usuario_id'] = $row['idUsuario'];
            
                $cartCookieName = "cart_user_" . $row['idUsuario'];
                if (isset($_COOKIE[$cartCookieName])) {
                    $_SESSION['cart'] = json_decode($_COOKIE[$cartCookieName], true);
                } else {
                    $_SESSION['cart'] = [];
                }
            
                header("Location: index.php");
                exit();
            } else {
                $error_message = 'Correo o contraseña incorrectos.';
            }
        } else {
            $error_message = 'Correo o contraseña incorrectos.';
        }
    } else {
        $error_message = 'Por favor, ingresa tu correo y contraseña.';
    }
}
?>


<?php include 'components/header.php'; ?>

<div class="min-h-screen bg-gray-50 flex flex-col justify-center py-12 sm:px-6 lg:px-8">
    <div class="sm:mx-auto sm:w-full sm:max-w-md">
        <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">
            Iniciar Sesión
        </h2>
        <p class="mt-2 text-center text-sm text-gray-600">
            ¿No tienes una cuenta?
            <a href="register.php" class="font-medium text-indigo-600 hover:text-indigo-500">
                Regístrate
            </a>
        </p>
    </div>

    <div class="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
        <div class="bg-white py-8 px-4 shadow sm:rounded-lg sm:px-10">
            <?php if ($error_message): ?>
                <div class="bg-red-100 text-red-700 p-3 mb-4 rounded">
                    <?php echo $error_message; ?>
                </div>
            <?php endif; ?>

            <form class="space-y-6" action="login.php" method="POST">
                <div>
                    <label for="correo" class="block text-sm font-medium text-gray-700">
                        Correo
                    </label>
                    <div class="mt-1">
                        <input id="correo" name="correo" type="email" required
                               class="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500">
                    </div>
                </div>

                <div>
                    <label for="contrasena" class="block text-sm font-medium text-gray-700">
                        Contraseña
                    </label>
                    <div class="mt-1">
                        <input id="contrasena" name="contrasena" type="password" required
                               class="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500">
                    </div>
                </div>
                <div>
                    <button type="submit"
                            class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500">
                        Iniciar Sesión
                    </button>
                </div>
            </form>
            <div class="mt-6">
                <div class="relative">
                    <div class="absolute inset-0 flex items-center">
                        <div class="w-full border-t border-gray-300"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<?php include 'components/footer.php'; ?>
