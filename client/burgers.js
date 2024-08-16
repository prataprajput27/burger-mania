const productWrapper = document.getElementById("product-wrapper");

function loadBurgers() {
  var user = JSON.parse(localStorage.getItem("user"));
  fetch("https://localhost:7257/api/BurgersApi", {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + user.token,
    },
  })
    .then((res) => res.json())
    .then((data) => {
      console.log(data);
      data.map((burgers) => {
        productWrapper.innerHTML += `
          <div class="card">
            <img id="product-image" src="${burgers.b_Image}" alt="">
            <div class="product-info">
              <h2 class="product-title">${burgers.b_Name}</h2>
              <p class="product-desc">${burgers.b_Description}</p>
              <p class="product-price" id="product-price- ${burgers.id}"> ₹ ${burgers.price}</p>
              <input class="product-quantity" id="product-quantity-${burgers.id}" type="number" placeholder="Quantity" min="1" max="5" required/>
              <button class="addToCartBtn" id="addToCartButton-${burgers.id}" onclick="addBurger('${burgers.id}')">Add to Cart</button>
            </div>
          </div>
        `;
      });
    });
}

var arr = [];

function addBurger(counter) {
  try {
    const userId = JSON.parse(localStorage.getItem("user")).user.id;
    const quantity = document.getElementById(
      `product-quantity-${counter}`
    ).value;

    if (quantity < 1 || quantity > 5) {
      alert("Quantity must be between 1 and 5 (inclusive).");
      return;
    }

    const burger = {
      burgerId: counter,
      quantity,
    };
    fetch(`https://localhost:7257/api/CartsApi/${userId}/addToCart`, {
      method: "PUT",
      body: JSON.stringify(burger),
      headers: {
        "Content-Type": "application/json",
      },
    })
      .then((res) => res.json())
      .then((data) => {
        localStorage.setItem("burgers", JSON.stringify(data));
        alert(`Burger added to the cart`);
        console.log(data);
      });
    if (counter >= 0 && counter < burgerData.length) {
      burgerData[counter]["quantity"] = document.getElementById(
        `product-quantity-${counter}`
      ).value;
      const burger = burgerData[counter];
      const arr = localStorage.getItem("burgers")
        ? JSON.parse(localStorage.getItem("burgers"))
        : [];
      arr.push(burger);
      localStorage.setItem("burgers", JSON.stringify(arr));
    }
  } catch (error) {
    console.log(error);
  }
}
