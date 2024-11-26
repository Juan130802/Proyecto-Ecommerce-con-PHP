<?php
function conectarDB() {
    $host = 'serverdes7.mysql.database.azure.com';
    $database = 'sistemacatalogo';
    $user = 'juanq13';
    $password = 'Comprometido13#';
    $ssl_ca = 'D:\\HP\\Nueva carpeta (2)\\DigiCertGlobalRootCA.crt.pem';

    $mysqli = mysqli_init();
    $mysqli->ssl_set(NULL, NULL, $ssl_ca, NULL, NULL);

    if (!$mysqli->real_connect($host, $user, $password, $database, 3306)) {
        die('Error de conexión: (' . $mysqli->connect_errno . ') ' . $mysqli->connect_error);
    }

    return $mysqli;
}

function insertarUsuario($nombre, $apellido, $correo, $contrasena, $tipoUsuario, $telefono, $direccion) {
    $mysqli = conectarDB();

    $nombre = $mysqli->real_escape_string($nombre);
    $apellido = $mysqli->real_escape_string($apellido);
    $correo = $mysqli->real_escape_string($correo);
    $contrasena = password_hash($contrasena, PASSWORD_DEFAULT);
    $tipoUsuario = $mysqli->real_escape_string($tipoUsuario);
    $telefono = $mysqli->real_escape_string($telefono);
    $direccion = $mysqli->real_escape_string($direccion);

    $query = "INSERT INTO usuarios (nombre, apellido, correo, contrasena, tipoUsuario, telefono, direccion) 
              VALUES (?, ?, ?, ?, ?, ?, ?)";

    if ($stmt = $mysqli->prepare($query)) {
        $stmt->bind_param("sssssss", $nombre, $apellido, $correo, $contrasena, $tipoUsuario, $telefono, $direccion);

        if ($stmt->execute()) {
            echo "Usuario registrado exitosamente.";
        } else {
            echo "Error al registrar el usuario: " . $stmt->error;
        }

        $stmt->close();
    } else {
        echo "Error al preparar la consulta: " . $mysqli->error;
    }

    $mysqli->close();
}
?>
