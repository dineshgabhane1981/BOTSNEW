let currentImage = 1;

// Ensure the first image is initially visible
document.getElementById('image1').style.display = 'block';

function toggleImages() {
  const image1 = document.getElementById('image1');
  const image2 = document.getElementById('image2');

  if (currentImage === 1) {
    image1.style.display = 'none';
    image2.style.display = 'block';
    currentImage = 2;

    // Change the background image of another page when image2 is toggled and place the path of your page on the place of another page
    document.querySelector('.another-page').style.backgroundImage = 'url(path_to_image_2)'; // Replace 'path_to_image_2' with your image URL
  } else {
    image1.style.display = 'block';
    image2.style.display = 'none';
    currentImage = 1;

    // Revert back the background image of another page when image1 is toggled and place the path of your page on the place of another page
    document.querySelector('.another-page').style.backgroundImage = 'url(path_to_image_1)'; // Replace 'path_to_image_1' with your image URL
  }
}

// Add event listeners to the images
document.getElementById('image1').addEventListener('click', toggleImages);
document.getElementById('image2').addEventListener('click', toggleImages);
