<?php
session_start();
require 'conexionBD.php';

if (!isset($_POST['usuario_id']) || empty($_POST['usuario_id'])) {
    die("Error: No se recibió el ID del usuario.");
}

$usuario_id = $_POST['usuario_id'];
echo "ID recibido: $usuario_id<br>";

error_reporting(E_ALL);
ini_set('display_errors', 1);

$mysqli = conectarDB();
if (!$mysqli) {
    die("Error en la conexión: " . $mysqli->connect_error);
}

$sqlCheck = "SELECT estado FROM usuarios WHERE idUsuario = ?";
$stmtCheck = $mysqli->prepare($sqlCheck);

if (!$stmtCheck) {
    die("Error al preparar la consulta de verificación: " . $mysqli->error);
}
$stmtCheck->bind_param("i", $usuario_id);
$stmtCheck->execute();
$resultCheck = $stmtCheck->get_result();

if ($resultCheck->num_rows > 0) {
    $row = $resultCheck->fetch_assoc();
    echo "Estado actual del usuario: " . $row['estado'] . "<br>";

    if ($row['estado'] === 'inactivo') {
        echo "El usuario ya está inactivo. No se realizarán cambios.<br>";
        $stmtCheck->close();
        $mysqli->close();
        exit();
    }
} else {
    echo "No se encontró ningún usuario con ID: $usuario_id.<br>";
    $stmtCheck->close();
    $mysqli->close();
    exit();
}
$stmtCheck->close();

$sqlUpdate = "UPDATE usuarios SET estado = 'inactivo' WHERE idUsuario = ?";
$stmtUpdate = $mysqli->prepare($sqlUpdate);

if (!$stmtUpdate) {
    die("Error al preparar la consulta de actualización: " . $mysqli->error);
}
echo "Consulta preparada para actualizar.<br>";

$stmtUpdate->bind_param("i", $usuario_id);
if ($stmtUpdate->execute()) {
    echo "Consulta ejecutada. Filas afectadas: " . $stmtUpdate->affected_rows . "<br>";
    if ($stmtUpdate->affected_rows > 0) {
        echo "Estado actualizado correctamente.<br>";
        session_destroy();
        header("Location: ../login.php ?mensaje=Tu cuenta ha sido dada de baja exitosamente.");
        exit();
    } else {
        echo "No se encontraron filas para actualizar. Verifica el ID.<br>";
    }
} else {
    die("Error al ejecutar la consulta: " . $stmtUpdate->error);
}

$stmtUpdate->close();
$mysqli->close();
?>