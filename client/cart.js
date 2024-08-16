function removeItem(id) {
  const userId = JSON.parse(localStorage.getItem("user")).user.id;

  fetch(`https://localhost:7257/api/CartsApi/${userId}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((res) => {
      if (!res.ok) {
        throw new Error("Network response was not ok");
      }
      return res.json();
    })
    .then((data) => {
      localStorage.setItem("burgers", JSON.stringify(data));
      const filteredItems = data.items.filter((item) => item.burgerId === id);

      if (filteredItems.length > 0) {
        const itemId = filteredItems[0].id;

        const cartItemArray = JSON.parse(
          localStorage.getItem("burgers")
        ).items.map((item) => item.id);

        if (cartItemArray.includes(itemId)) {
          fetch(
            `https://localhost:7257/api/CartsApi/${userId}/removeFromCart/${itemId}`,
            {
              method: "PUT",
              headers: {
                "Content-Type": "application/json",
              },
            }
          )
            .then((res) => {
              if (!res.ok) {
                throw new Error("Network response was not ok");
              }
              return res.json();
            })
            .then((data) => {
              console.log(data);
              localStorage.setItem("burgers", JSON.stringify(data));
              onLoadOrder();
              window.location.href = "cart.html";
            })
            .catch((error) => {
              console.error("Error removing item from cart:", error);
            });
        } else {
          console.log(`Item with id ${itemId} is not in the cart.`);
        }
      } else {
        console.log("No items found with the specified burgerId.");
      }
    })
    .catch((error) => {
      console.error("Error fetching cart data:", error);
    });
}

function onLoadOrder() {
  const userId = JSON.parse(localStorage.getItem("user")).user.id;

  fetch(`https://localhost:7257/api/CartsApi/${userId}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((res) => res.json())
    .then((d) => {
      var tbodyContent = "";
      var totalPrice = 0;
      var totalOriginalPrice = 0;

      d.items.forEach((item) => {
        var burgerName = item.burger.b_Name;
        var burgerImage = item.burger.b_Image;
        var burgerDescription = item.burger.b_Description;
        var burgerPrice = parseFloat(item.burger.price);
        var burgerId = item.burger.id;
        var quantity = item.quantity;

        var originalPrice = burgerPrice * quantity;
        totalOriginalPrice += originalPrice;
        totalPrice += originalPrice;

        tbodyContent += `
          <tr>
            <td style="text-align: center">
              <img src="${burgerImage}" width="150" height="50" style="float: left; object-fit: cover"></img>
            </td>
            <td>
              ${burgerName}
              ${burgerDescription}
            </td>
            <td>${quantity}</td>
            <td>${burgerPrice}</td>
            <td>${originalPrice.toFixed(2)}</td>
            <td>
              <button class="remove-button" onclick="removeItem('${burgerId}')">Remove</button>
            </td>
          </tr>`;
      });

      document.getElementById("tbody").innerHTML = tbodyContent;

      var gst = 5;
      var serviceTax = 5;
      var gstAmount = totalPrice * (gst / 100);
      var serviceTaxAmount = totalPrice * (serviceTax / 100);
      totalPrice += gstAmount + serviceTaxAmount;

      var discount = 0;
      if (totalPrice >= 1000) {
        discount = 10;
        totalPrice -= totalPrice * (discount / 100);
      } else if (totalPrice >= 500 && totalPrice < 1000) {
        discount = 5;
        totalPrice -= totalPrice * (discount / 100);
      }

      document.getElementById("order-summary").innerHTML = `
        <div style="display: flex; flex-direction: column; align-items: flex-start;">
          <h4>Bill Summary</h4>
          <div>Original Price: Rs. <span class="highlight">${totalOriginalPrice.toFixed(
            2
          )}</span></div>
          <div>GST (5%): Rs. <span class="highlight">${gstAmount.toFixed(
            2
          )}</span></div>
          <div>Service Tax (5%): Rs. <span class="highlight">${serviceTaxAmount.toFixed(
            2
          )}</span></div>
          <div style="font-weight: bold;">Total Price (after taxes): Rs. <span class="highlight">${(
            totalOriginalPrice +
            gstAmount +
            serviceTaxAmount
          ).toFixed(2)}</span></div>
          <div>Discount: <span class="highlight">${discount}%</span></div>
          <div style="font-weight: bold;">Total Price after Discount: Rs. <span class="highlight">${totalPrice.toFixed(
            2
          )}</span></div>
          <button class="order-button" id="order">Order Now</button>
        </div>`;

      var modal = document.getElementById("myModal");
      var span = document.getElementsByClassName("close")[0];
      var btn = document.getElementById("order");

      span.onclick = function () {
        modal.style.display = "none";
        window.location.href = "index.html";
      };

      btn.addEventListener("click", function () {
        modal.style.display = "block";
      });
    });
}
