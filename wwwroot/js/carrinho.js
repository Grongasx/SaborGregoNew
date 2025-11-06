
let cart = [];

function handleAddToCart(event, productName, productPrice) {
    event.preventDefault();
    addToCart(productName, productPrice);
}

function addToCart(productName, productPrice) {
    const product = cart.find(p => p.name === productName);

    if (product) {
        product.quantity++;
    } else {
        cart.push({ name: productName, price: productPrice, quantity: 1 });
    }

    updateCart();
    showToast(`${productName} foi adicionado ao carrinho.`);
}

function removeFromCart(productName) {
    const productIndex = cart.findIndex(p => p.name === productName);

    if (productIndex > -1) {
        cart.splice(productIndex, 1);
    }

    updateCart();
}

function increaseQuantity(productName) {
    const product = cart.find(p => p.name === productName);
    if (product) {
        product.quantity++;
    }
    updateCart();
}

function decreaseQuantity(productName) {
    const product = cart.find(p => p.name === productName);
    if (product && product.quantity > 1) {
        product.quantity--;
    } else {
        removeFromCart(productName);
    }
    updateCart();
}

function clearCart() {
    cart = [];
    updateCart();
}

function updateCart() {
    const cartItems = document.getElementById('cart-items');
    cartItems.innerHTML = '';

    cart.forEach(item => {
        const li = document.createElement('li');
        li.className = 'list-group-item d-flex justify-content-between align-items-center';
        li.innerHTML = `
            <span>${item.name} - ${item.price.toFixed(2)}</span>
            <div>
                <button class="btn btn-secondary btn-sm" onclick="decreaseQuantity('${item.name}')">-</button>
                <span class="mx-2">${item.quantity}</span>
                <button class="btn btn-secondary btn-sm" onclick="increaseQuantity('${item.name}')">+</button>
                <button class="btn btn-danger btn-sm ms-2" onclick="removeFromCart('${item.name}')">Remover</button>
            </div>
        `;
        cartItems.appendChild(li);
    });

    const total = cart.reduce((acc, item) => acc + (item.price * item.quantity), 0);
    document.getElementById('cart-total').innerText = total.toFixed(2);

    localStorage.setItem('cart', JSON.stringify(cart));
    updateCartCount();
}

function loadCart() {
    const savedCart = localStorage.getItem('cart');

    if (savedCart) {
        cart = JSON.parse(savedCart);
        updateCart();
    }
}

function showToast(message) {
    const toastContainer = document.getElementById('toast-container');
    const toast = document.createElement('div');
    toast.className = 'toast align-items-center text-white bg-primary border-0 show';
    toast.setAttribute('role', 'alert');
    toast.setAttribute('aria-live', 'assertive');
    toast.setAttribute('aria-atomic', 'true');

    toast.innerHTML = `
        <div class="d-flex">
            <div class="toast-body">
                ${message}
            </div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
        </div>
    `;

    toastContainer.appendChild(toast);

    const bsToast = new bootstrap.Toast(toast);
    bsToast.show();

    setTimeout(() => {
        bsToast.hide();
        toast.remove();
    }, 3000);
}

loadCart();
