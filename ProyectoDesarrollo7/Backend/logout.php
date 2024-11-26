<?php
session_start();

if (isset($_SESSION['usuario_id']) && isset($_SESSION['cart'])) {
    $userId = $_SESSION['usuario_id'];
    $cartCookieName = "cart_user_" . $userId;
    setcookie($cartCookieName, json_encode($_SESSION['cart']), time() + (86400 * 30), "/"); // Validez de 30 días
}

session_unset();
session_destroy();

header("Location: ../login.php");
exit();
?>
