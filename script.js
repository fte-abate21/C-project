// Function to show the selected section
function showSection(id) {
  const sections = document.querySelectorAll('.content-section');
  sections.forEach(section => {
    section.classList.remove('active');
  });

  const selectedSection = document.getElementById(id);
  if (selectedSection) {
    selectedSection.classList.add('active');
  }
}

// Default to show "Home" section
document.addEventListener("DOMContentLoaded", () => {
  showSection('home');

  // Handle the borrow form submission
  const borrowForm = document.getElementById("borrowForm");
  const confirmationMessage = document.getElementById("confirmationMessage");

  if (borrowForm) {
    borrowForm.addEventListener("submit", function (e) {
      e.preventDefault();
      confirmationMessage.classList.remove("hidden");
      borrowForm.reset();
    });
  }
});

// Dark Mode Toggle
function toggleDarkMode() {
  document.body.classList.toggle("dark-mode");
}
