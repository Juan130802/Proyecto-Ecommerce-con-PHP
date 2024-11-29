<?php
session_start();
require 'Backend/conexionBD.php';

if (!isset($_SESSION['usuario_id'])) {
    die("No tienes permiso para acceder a esta página.");
}

$userId = $_SESSION['usuario_id'];
$mysqli = conectarDB();

$limite = 10;
$pagina = isset($_GET['pagina']) ? (int)$_GET['pagina'] : 1;
$inicio = ($pagina - 1) * $limite;

$queryUsuarios = "SELECT idUsuario, tipo_usuario, nombre, apellido, telefono, direccion, estado, correo FROM usuarios LIMIT ?, ?";
$stmtUsuarios = $mysqli->prepare($queryUsuarios);
$stmtUsuarios->bind_param("ii", $inicio, $limite);
$stmtUsuarios->execute();
$resultUsuarios = $stmtUsuarios->get_result();

if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['eliminar'])) {
    $idUsuario = $_POST['idUsuario'];

    $queryDelete = "DELETE FROM usuarios WHERE idUsuario = ?";
    $stmtDelete = $mysqli->prepare($queryDelete);
    $stmtDelete->bind_param("i", $idUsuario);
    if ($stmtDelete->execute()) {
        echo "<script>alert('Usuario eliminado correctamente.');</script>";
        echo "<script>window.location.href = window.location.href;</script>";
        exit;
    } else {
        echo "<script>alert('Error al eliminar el usuario.');</script>";
    }
}

if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['actualizar'])) {
    $idUsuario = $_POST['idUsuario'];

    $queryOriginal = "SELECT * FROM usuarios WHERE idUsuario = ?";
    $stmtOriginal = $mysqli->prepare($queryOriginal);
    $stmtOriginal->bind_param("i", $idUsuario);
    $stmtOriginal->execute();
    $usuarioOriginal = $stmtOriginal->get_result()->fetch_assoc();

    $tipo_usuario = $_POST['tipo_usuario'];
    $nombre = $_POST['nombre'];
    $apellido = $_POST['apellido'];
    $telefono = $_POST['telefono'];
    $direccion = $_POST['direccion'];
    $correo = $_POST['correo'];
    $estado = $_POST['estado'];

    $cambios = [];
    if ($tipo_usuario !== $usuarioOriginal['tipo_usuario']) $cambios['tipo_usuario'] = $tipo_usuario;
    if ($nombre !== $usuarioOriginal['nombre']) $cambios['nombre'] = $nombre;
    if ($apellido !== $usuarioOriginal['apellido']) $cambios['apellido'] = $apellido;
    if ($telefono !== $usuarioOriginal['telefono']) $cambios['telefono'] = $telefono;
    if ($direccion !== $usuarioOriginal['direccion']) $cambios['direccion'] = $direccion;
    if ($correo !== $usuarioOriginal['correo']) $cambios['correo'] = $correo;
    if ($estado !== $usuarioOriginal['estado']) $cambios['estado'] = $estado;

    if (empty($cambios)) {
        echo "<script>alert('No se detectaron cambios para actualizar.');</script>";
    } else {
        $sets = [];
        $params = [];
        $types = '';

        foreach ($cambios as $campo => $valor) {
            $sets[] = "$campo = ?";
            $params[] = $valor;
            $types .= 's';
        }

        $params[] = $idUsuario;
        $types .= 'i';
        $queryUpdate = "UPDATE usuarios SET " . implode(", ", $sets) . " WHERE idUsuario = ?";
        $stmtUpdate = $mysqli->prepare($queryUpdate);
        $stmtUpdate->bind_param($types, ...$params);

        if ($stmtUpdate->execute()) {
            echo "<script>alert('Usuario actualizado correctamente.');</script>";
            echo "<script>window.location.href = window.location.href;</script>";
        } else {
            echo "<script>alert('Error al actualizar el usuario.');</script>";
        }
    }
}


$queryCount = "SELECT COUNT(*) as total FROM usuarios";
$resultCount = $mysqli->query($queryCount);
$totalUsuarios = $resultCount->fetch_assoc()['total'];
$totalPaginas = ceil($totalUsuarios / $limite);

function traducirEstado($estado) {
    $estados = [
        'activo' => 'Activo',
        'inactivo' => 'Inactivo',
    ];
    return $estados[$estado] ?? 'Desconocido';
}
?>

<?php include 'components/header.php'; ?>
<?php include 'components/navbar.php'; ?>

<div class="min-h-screen bg-gray-50 py-12">
    <div class="container mx-auto px-4">
        <h1 class="text-3xl font-bold mb-8">Lista de Usuarios</h1>

        <!-- Formulario de Crear Usuario -->
        <h2 class="text-2xl font-semibold mb-6">Crear Nuevo Usuario</h2>
        <form method="POST" action="" class="bg-white p-6 rounded-lg shadow-md mb-8">
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <select name="tipo_usuario" class="w-full px-4 py-2 border rounded">
                    <option value="cliente">Cliente</option>
                    <option value="administrador">Administrador</option>
                </select>
                <input type="text" name="nombre" placeholder="Nombre" class="w-full px-4 py-2 border rounded">
                <input type="text" name="apellido" placeholder="Apellido" class="w-full px-4 py-2 border rounded">
                <input type="text" name="telefono" placeholder="Teléfono" class="w-full px-4 py-2 border rounded">
                <input type="text" name="direccion" placeholder="Dirección" class="w-full px-4 py-2 border rounded">
                <select name="estado" class="w-full px-4 py-2 border rounded">
                    <option value="activo">Activo</option>
                    <option value="inactivo">Inactivo</option>
                </select>
                <input type="email" name="correo" placeholder="Correo Electrónico" class="w-full px-4 py-2 border rounded">
                <input type="password" name="contrasena" placeholder="Contraseña" class="w-full px-4 py-2 border rounded">
            </div>
            <button type="submit" name="crear" class="bg-green-500 text-white px-4 py-2 rounded mt-4 hover:bg-green-600">Crear Usuario</button>
        </form>

        <?php if ($resultUsuarios->num_rows > 0): ?>
            <div class="bg-white rounded-lg shadow-md p-4">
                <table class="w-full border-collapse text-sm">
                    <thead>
                        <tr class="bg-gray-200 text-xs text-left">
                            <th class="border px-2 py-1">ID Usuario</th>
                            <th class="border px-2 py-1">Tipo de Usuario</th>
                            <th class="border px-2 py-1">Nombre</th>
                            <th class="border px-2 py-1">Apellido</th>
                            <th class="border px-2 py-1">Teléfono</th>
                            <th class="border px-2 py-1">Dirección</th>
                            <th class="border px-2 py-1">Correo</th>
                            <th class="border px-2 py-1">Estado</th>
                            <th class="border px-2 py-1">Actualizar</th>
                            <th class="border px-2 py-1">Eliminar</th>
                        </tr>
                    </thead>
                    <tbody>
                        <?php while ($usuario = $resultUsuarios->fetch_assoc()): ?>
                            <tr class="hover:bg-gray-100">
                                <form method="POST" action="">
                                    <input type="hidden" name="idUsuario" value="<?php echo $usuario['idUsuario']; ?>">
                                    <td class="border px-2 py-1"><?php echo $usuario['idUsuario']; ?></td>
                                    <td class="border px-2 py-1">
                                        <input type="text" name="tipo_usuario" value="<?php echo $usuario['tipo_usuario']; ?>" class="w-full px-2 py-1">
                                    </td>
                                    <td class="border px-2 py-1">
                                        <input type="text" name="nombre" value="<?php echo $usuario['nombre']; ?>" class="w-full px-2 py-1">
                                    </td>
                                    <td class="border px-2 py-1">
                                        <input type="text" name="apellido" value="<?php echo $usuario['apellido']; ?>" class="w-full px-2 py-1">
                                    </td>
                                    <td class="border px-2 py-1">
                                        <input type="text" name="telefono" value="<?php echo $usuario['telefono']; ?>" class="w-full px-2 py-1">
                                    </td>
                                    <td class="border px-2 py-1">
                                        <input type="text" name="direccion" value="<?php echo $usuario['direccion']; ?>" class="w-full px-2 py-1">
                                    </td>
                                    <td class="border px-2 py-1">
                                        <input type="text" name="correo" value="<?php echo $usuario['correo']; ?>" class="w-full px-2 py-1">
                                    </td>
                                    <td class="border px-2 py-1">
                                        <select name="estado" class="w-full px-2 py-1">
                                            <option value="activo" <?php echo $usuario['estado'] == 'activo' ? 'selected' : ''; ?>>Activo</option>
                                            <option value="inactivo" <?php echo $usuario['estado'] == 'inactivo' ? 'selected' : ''; ?>>Inactivo</option>
                                        </select>
                                    </td>
                                    <td class="border px-2 py-1">
                                        <button type="submit" name="actualizar" class="bg-blue-500 text-white px-4 py-1 rounded hover:bg-blue-600">Actualizar</button>
                                    </td>
                                </form>
                                <td class="border px-2 py-1">
                                    <!-- Botón de eliminar -->
                                    <form method="POST" action="">
                                        <input type="hidden" name="idUsuario" value="<?php echo $usuario['idUsuario']; ?>">
                                        <button type="submit" name="eliminar" class="bg-red-500 text-white px-4 py-1 rounded hover:bg-red-600">Eliminar</button>
                                    </form>
                                </td>
                            </tr>
                        <?php endwhile; ?>
                    </tbody>
                </table>
            </div>
            <!-- Paginación -->
            <div class="mt-4">
                <nav class="flex justify-center">
                    <ul class="flex space-x-2">
                        <?php for ($i = 1; $i <= $totalPaginas; $i++): ?>
                            <li>
                                <a href="?pagina=<?php echo $i; ?>" class="px-4 py-2 border rounded <?php echo ($i === $pagina) ? 'bg-blue-500 text-white' : 'text-blue-500'; ?>"><?php echo $i; ?></a>
                            </li>
                        <?php endfor; ?>
                    </ul>
                </nav>
            </div>
        <?php else: ?>
            <p>No hay usuarios registrados.</p>
        <?php endif; ?>
    </div>
</div>
<?php include 'components/footer.php'; ?>
