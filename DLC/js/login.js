function sendOTP() {
  const mobileNumber = document.getElementById("signup-mobile").value;
  const otp = generateOTP();
  sessionStorage.setItem("mobileNumber", mobileNumber);
  sessionStorage.setItem("otp", otp);

  window.location.href = "otp-verification.html";
}

function generateOTP() {
  return Math.floor(100000 + Math.random() * 900000);
}
