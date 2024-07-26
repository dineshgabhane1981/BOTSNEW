document.addEventListener('DOMContentLoaded', function () {
    // Open Popup
    document.getElementById('openPopupBtn1').addEventListener('click', function () {
      document.getElementById('popupOverlay1').style.display = 'block';
      document.getElementById('popupBox1').style.display = 'flex';
    });
  
    // Close Popup
    document.getElementById('closePopupBtn1').addEventListener('click', function () {
      document.getElementById('popupOverlay1').style.display = 'none';
      document.getElementById('popupBox1').style.display = 'none';
    });
  
    // Submit Logic
    document.getElementById('submitPopupBtn1').addEventListener('click', function () {
      var selectedOption = document.querySelector('input[name="continue"]:checked');
  
      if (selectedOption) {
        var value = selectedOption.value;
        console.log('Selected option:', value);
      } else {
        console.log('No option selected.');
      }
    });
  });
  