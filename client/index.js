var modal = document.getElementById("myModal");
var btn = document.getElementById("modal_open");
var span = document.getElementsByClassName("close")[0];

btn.addEventListener("click", function () {
  modal.style.display = "block";
});

span.onclick = function () {
  modal.style.display = "none";
};

window.onclick = function (event) {
  if (event.target == modal) {
    modal.style.display = "none";
  }
};

document.getElementById("showLogin").addEventListener("click", function (e) {
  e.preventDefault();
  document.getElementById("registerForm").style.display = "none";
  document.getElementById("loginForm").style.display = "block";
});

document.getElementById("showRegister").addEventListener("click", function (e) {
  e.preventDefault();
  document.getElementById("loginForm").style.display = "none";
  document.getElementById("registerForm").style.display = "block";
});

document.getElementById("registerForm").addEventListener("submit", (e) => {
  e.preventDefault();
  const tel = document.getElementById("tel").value;
  const username = document.getElementById("username").value;
  const email = document.getElementById("email").value;
  const password = document.getElementById("password").value;
  const phoneRegex = /^\d{10}$/;
  const emailRegex = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
  const passwordRegex = /^.{8,}$/;
  const user = {
    username,
    email,
    phone: tel,
    password,
  };

  let isValid = true;
  const errorMessagesDiv = document.getElementById("errorMessages");
  errorMessagesDiv.innerHTML = "";

  if (!phoneRegex.test(tel)) {
    errorMessagesDiv.innerHTML +=
      "<p>Please enter a valid 10-digit mobile number.</p>";
    isValid = false;
  }

  if (!emailRegex.test(email)) {
    errorMessagesDiv.innerHTML += "<p>Please enter a valid email address.</p>";
    isValid = false;
  }

  if (!passwordRegex.test(password)) {
    errorMessagesDiv.innerHTML +=
      "<p>Password must be at least 8 characters long.</p>";
    isValid = false;
  }

  if (isValid) {
    fetch("https://localhost:7257/api/Users", {
      method: "POST",
      body: JSON.stringify(user),
      headers: {
        "Content-Type": "application/json",
      },
    })
      .then((res) => res.json())
      .then((data) => {
        console.log(data);
        localStorage.setItem("user", JSON.stringify(data));
        alert("Registration successful!");
        document.getElementById("registerForm").style.display = "none";
        document.getElementById("loginForm").style.display = "block";
        // window.location.replace("burgers.html");
      });
  }
});

// login form submission
document.getElementById("loginForm").addEventListener("submit", (e) => {
  e.preventDefault();
  const email = document.getElementById("loginEmail").value;
  const password = document.getElementById("loginPassword").value;

  fetch("https://localhost:7257/api/Auth/login", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ email, password }),
  })
    .then((res) => {
      if (!res.ok) {
        throw new Error("Invalid credentials");
      }
      return res.json();
    })
    .then((data) => {
      if (data.token) {
        localStorage.setItem("token", data.token);
        localStorage.setItem("user", JSON.stringify(data));
        window.location.replace("burgers.html");
      } else {
        displayLoginError(
          "Login failed: " + (data.message || "Invalid credentials")
        );
      }
    })
    .catch((error) => {
      displayLoginError(error.message);
    });
});

function displayLoginError(message) {
  const errorMessagesDiv = document.getElementById("loginErrorMessages");
  errorMessagesDiv.innerHTML = message;
}
