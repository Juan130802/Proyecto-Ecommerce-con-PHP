<?php
session_start();
include 'Backend/conexionBD.php';

$mysqli = conectarDB();

if (!isset($_SESSION['usuario_id'])) {
    header("Location: login.php");
    exit();
}

$usuario_id = $_SESSION['usuario_id'];

$sql = "SELECT * FROM usuarios WHERE idUsuario = ?";
$stmt = $mysqli->prepare($sql);
$stmt->bind_param("i", $usuario_id);
$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    $usuario = $result->fetch_assoc();
} else {
    session_destroy();
    header("Location: login.php");
    exit();
}
?>

<?php
if (isset($_SESSION['mensaje'])) {
    echo "<div class='bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded relative' role='alert'>
            {$_SESSION['mensaje']}
          </div>";
    unset($_SESSION['mensaje']);
}

if (isset($_SESSION['error'])) {
    echo "<div class='bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative' role='alert'>
            {$_SESSION['error']}
          </div>";
    unset($_SESSION['error']);
}
?>


<?php include 'components/header.php'; ?>
<?php include 'components/navbar.php'; ?>

<div class="container mx-auto px-4 py-10">
    <div class="bg-white shadow-md rounded-lg p-6">
        <h2 class="text-2xl font-bold mb-6 text-gray-700">Mi Cuenta</h2>
        <form action="Backend/actualizar_datos.php" method="POST">
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div>
                    <label class="block text-gray-600 font-semibold">Nombre</label>
                    <input type="text" name="nombre" class="bg-gray-100 px-4 py-2 rounded-lg w-full"
                        value="<?php echo htmlspecialchars($usuario['nombre']); ?>" required>
                </div>
                <div>
                    <label class="block text-gray-600 font-semibold">Apellido</label>
                    <input type="text" name="apellido" class="bg-gray-100 px-4 py-2 rounded-lg w-full"
                        value="<?php echo htmlspecialchars($usuario['apellido']); ?>" required>
                </div>
                <div>
                    <label class="block text-gray-600 font-semibold">Correo</label>
                    <input type="email" name="correo" class="bg-gray-100 px-4 py-2 rounded-lg w-full"
                        value="<?php echo htmlspecialchars($usuario['correo']); ?>" required>
                </div>
                <div>
                    <label class="block text-gray-600 font-semibold">Teléfono</label>
                    <input type="text" name="telefono" class="bg-gray-100 px-4 py-2 rounded-lg w-full"
                        value="<?php echo htmlspecialchars($usuario['telefono']); ?>">
                </div>
                <div>
                    <label class="block text-gray-600 font-semibold">Dirección</label>
                    <input type="text" name="direccion" class="bg-gray-100 px-4 py-2 rounded-lg w-full"
                        value="<?php echo htmlspecialchars($usuario['direccion']); ?>">
                </div>
                <div>
                    <label class="block text-gray-600 font-semibold">Contraseña Actual</label>
                    <div class="relative">
                        <input type="text" id="passwordField" class="bg-gray-100 px-4 py-2 rounded-lg w-full"
                            value="<?php echo htmlspecialchars($usuario['contrasena']); ?>" readonly>
                        <button type="button" onclick="togglePassword()"
                            class="absolute right-3 top-2.5 text-gray-600">Mostrar</button>
                    </div>
                </div>
                <div>
                    <label class="block text-gray-600 font-semibold">Nueva Contraseña</label>
                    <input type="password" name="contrasena" class="bg-gray-100 px-4 py-2 rounded-lg w-full"
                        placeholder="Deja vacío si no deseas cambiarla">
                    <p class="text-sm text-gray-500 mt-2">Si no deseas cambiarla, deja este campo vacío.</p>
                </div>
                <div class="md:col-span-2">
                    <button type="submit" class="bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600">
                        Actualizar
                    </button>
                </div>
                <input type="hidden" name="usuario_id" value="<?php echo htmlspecialchars($usuario_id); ?>">
                <p>ID Usuario enviado: <?php echo htmlspecialchars($usuario_id); ?></p>
            </div>
        </form>
        <form action="Backend/dar_baja.php" method="POST"
            onsubmit="return confirm('¿Estás seguro de que deseas dar de baja tu cuenta? Esta acción no se puede deshacer.')">
            <input type="hidden" name="usuario_id" value="<?php echo htmlspecialchars($usuario_id); ?>">
            <button type="submit" class="bg-red-500 text-white px-4 py-2 rounded-lg hover:bg-red-600">
                Dar de Baja
            </button>
        </form>
    </div>
</div>
<script>
    function togglePassword() {
        const passwordField = document.getElementById('passwordField');
        const button = passwordField.nextElementSibling;
        if (passwordField.type === 'password') {
            passwordField.type = 'text';
            button.textContent = 'Ocultar';
        } else {
            passwordField.type = 'password';
            button.textContent = 'Mostrar';
        }
    }
</script>

<?php include 'components/footer.php'; ?>