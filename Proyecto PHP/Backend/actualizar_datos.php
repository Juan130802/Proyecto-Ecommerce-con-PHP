<?php
session_start();
require_once 'conexionBD.php';

$mysqli = conectarDB();

if (!isset($_SESSION['usuario_id'])) {
    header("Location: login.php");
    exit();
}

$usuario_id = $_SESSION['usuario_id'];

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $nuevo_nombre = $_POST['nombre'] ?? null;
    $nuevo_apellido = $_POST['apellido'] ?? null;
    $nuevo_telefono = $_POST['telefono'] ?? null;
    $nueva_direccion = $_POST['direccion'] ?? null;
    $nueva_contrasena = $_POST['contrasena'] ?? null;

    $campos_a_actualizar = [];
    $parametros = [];
    $tipos = '';

    if (!empty($nuevo_nombre)) {
        $campos_a_actualizar[] = "nombre = ?";
        $parametros[] = $nuevo_nombre;
        $tipos .= 's';
    }
    if (!empty($nuevo_apellido)) {
        $campos_a_actualizar[] = "apellido = ?";
        $parametros[] = $nuevo_apellido;
        $tipos .= 's';
    }
    if (!empty($nuevo_telefono)) {
        $campos_a_actualizar[] = "telefono = ?";
        $parametros[] = $nuevo_telefono;
        $tipos .= 's';
    }
    if (!empty($nueva_direccion)) {
        $campos_a_actualizar[] = "direccion = ?";
        $parametros[] = $nueva_direccion;
        $tipos .= 's';
    }
    if (!empty($nueva_contrasena)) {
        $campos_a_actualizar[] = "contrasena = ?";
        $parametros[] = password_hash($nueva_contrasena, PASSWORD_BCRYPT);
        $tipos .= 's';
    }

    if (!empty($campos_a_actualizar)) {
        $parametros[] = $usuario_id;
        $tipos .= 'i';
    
        $sql = "UPDATE usuarios SET " . implode(", ", $campos_a_actualizar) . " WHERE idUsuario = ?";
        $stmt = $mysqli->prepare($sql);
    
        if ($stmt) {
            $stmt->bind_param($tipos, ...$parametros);
    
            if ($stmt->execute()) {
                $_SESSION['mensaje'] = "Datos actualizados correctamente.";
            } else {
                $_SESSION['error'] = "Error al actualizar los datos: " . $stmt->error;
            }
            $stmt->close();
        } else {
            $_SESSION['error'] = "Error al preparar la consulta: " . $mysqli->error;
        }
    } else {
        $_SESSION['mensaje'] = "No se realizaron cambios en los datos.";
    }
    header("Location: ../cuenta.php");
    exit();
    
}
?>
