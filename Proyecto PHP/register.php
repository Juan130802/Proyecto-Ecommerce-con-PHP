<?php 
include 'components/header.php'; 
include 'components/navbar.php'; 
include 'Backend/conexionBD.php';

if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    $nombre = $_POST['nombre'];
    $apellido = $_POST['apellido'];
    $correo = $_POST['correo'];
    $contrasena = $_POST['contrasena'];
    $contrasena_confirmation = $_POST['contrasena_confirmation'];
    $tipoUsuario = $_POST['tipoUsuario'];
    $telefono = $_POST['telefono'];
    $direccion = $_POST['direccion'];
    if ($contrasena !== $contrasena_confirmation) {
        echo "Las contraseñas no coinciden.";
    } else {
        if (!filter_var($correo, FILTER_VALIDATE_EMAIL)) {
            echo "Correo electrónico inválido.";
        } else {
            $mysqli = conectarDB();

            $stmt = $mysqli->prepare("SELECT * FROM usuarios WHERE correo = ?");
            $stmt->bind_param("s", $correo);
            $stmt->execute();
            $result = $stmt->get_result();

            if ($result->num_rows > 0) {
                echo "El correo electrónico ya está registrado.";
            } else {
                $contrasena_encriptada = password_hash($contrasena, PASSWORD_BCRYPT);

                $usuario = strtolower($nombre[0] . $apellido . rand(100, 999));

                $stmt = $mysqli->prepare("INSERT INTO usuarios (usuario, nombre, apellido, correo, contrasena, tipo_usuario, telefono, direccion) VALUES (?, ?, ?, ?, ?, ?, ?, ?)");
                $stmt->bind_param("ssssssss", $usuario, $nombre, $apellido, $correo, $contrasena_encriptada, $tipoUsuario, $telefono, $direccion);

                if ($stmt->execute()) {
                    echo "Registro exitoso. Tu nombre de usuario es: " . $usuario . ". <a href='login.php'>Inicia sesión</a>";
                } else {
                    echo "Error al registrar el usuario: " . $stmt->error;
                }
            }
            $stmt->close();
            $mysqli->close();
        }
    }
}
?>

<div class="min-h-screen bg-gray-50 flex flex-col justify-center py-12 sm:px-6 lg:px-8">
    <div class="sm:mx-auto sm:w-full sm:max-w-md">
        <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">
            Crear una cuenta
        </h2>
        <p class="mt-2 text-center text-sm text-gray-600">
            ¿Ya tienes una cuenta?
            <a href="login.php" class="font-medium text-indigo-600 hover:text-indigo-500">
                Inicia sesión
            </a>
        </p>
    </div>

    <div class="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
        <div class="bg-white py-8 px-4 shadow sm:rounded-lg sm:px-10">
            <form class="space-y-6" action="register.php" method="POST">
                <div class="grid grid-cols-1 gap-6">
                    <div class="col-span-1">
                        <label for="nombre" class="block text-sm font-medium text-gray-700">
                            Nombre completo
                        </label>
                        <input type="text" name="nombre" id="nombre" required
                               class="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500">
                    </div>

                    <div class="col-span-1">
                        <label for="apellido" class="block text-sm font-medium text-gray-700">
                            Apellido
                        </label>
                        <input type="text" name="apellido" id="apellido" required
                               class="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500">
                    </div>

                    <div class="col-span-1">
                        <label for="correo" class="block text-sm font-medium text-gray-700">
                            Correo electrónico
                        </label>
                        <input type="email" name="correo" id="correo" 
                               class="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500">
                    </div>

                    <div class="col-span-1">
                        <label for="contrasena" class="block text-sm font-medium text-gray-700">
                            Contraseña
                        </label>
                        <input type="password" name="contrasena" id="contrasena" required
                               class="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500">
                    </div>

                    <div class="col-span-1">
                        <label for="contrasena_confirmation" class="block text-sm font-medium text-gray-700">
                            Confirmar contraseña
                        </label>
                        <input type="password" name="contrasena_confirmation" id="contrasena_confirmation" required
                               class="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500">
                    </div>

                    <div class="col-span-1">
                        <label for="tipoUsuario" class="block text-sm font-medium text-gray-700">
                            Tipo de usuario
                        </label>
                        <select name="tipoUsuario" id="tipoUsuario" required
                                class="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500">
                            <option value="administrador">Administrador</option>
                            <option value="cliente">Cliente</option>
                        </select>
                    </div>

                    <div class="col-span-1">
                        <label for="telefono" class="block text-sm font-medium text-gray-700">
                            Teléfono
                        </label>
                        <input type="text" name="telefono" id="telefono" 
                               class="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500">
                    </div>

                    <div class="col-span-1">
                        <label for="direccion" class="block text-sm font-medium text-gray-700">
                            Dirección
                        </label>
                        <textarea name="direccion" id="direccion" rows="3" 
                                  class="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"></textarea>
                    </div>
                </div>

                <div class="flex items-center">
                    <input id="terms" name="terms" type="checkbox" required
                           class="h-4 w-4 text-indigo-600 focus:ring-indigo-500 border-gray-300 rounded">
                    <label for="terms" class="ml-2 block text-sm text-gray-900">
                        Acepto los <a href="#" class="text-indigo-600 hover:text-indigo-500">términos y condiciones</a>
                    </label>
                </div>

                <div>
                    <button type="submit"
                            class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500">
                        Registrarse
                    </button>
                </div>
            </form>
        </div>
    </div>
</div>

<?php include 'components/footer.php'; ?>
