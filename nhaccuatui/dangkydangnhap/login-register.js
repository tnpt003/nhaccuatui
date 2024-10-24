
document.getElementById('registerBtn').addEventListener('click', function () {
    document.getElementById('registerForm').classList.add('active');
    document.getElementById('loginForm').classList.remove('active');
});

document.getElementById('loginBtn').addEventListener('click', function () {
    document.getElementById('loginForm').classList.add('active');
    document.getElementById('registerForm').classList.remove('active');
});

document.getElementById('showRegister').addEventListener('click', function () {
    document.getElementById('registerForm').classList.add('active');
    document.getElementById('loginForm').classList.remove('active');
});

document.getElementById('showLogin').addEventListener('click', function () {
    document.getElementById('loginForm').classList.add('active');
    document.getElementById('registerForm').classList.remove('active');
});


