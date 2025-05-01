// Mobile menu functionality
function showMenu() {
    const navLinks = document.getElementById("navLinks")
    navLinks.style.right = "0"
}

function hideMenu() {
    const navLinks = document.getElementById("navLinks")
    navLinks.style.right = "-200px"
}

// Add responsive behavior for mobile
document.addEventListener("DOMContentLoaded", () => {
    // Check if we're on a mobile device
    const isMobile = window.matchMedia("(max-width: 768px)").matches

    if (isMobile) {
        // Add click handlers for dropdown toggles on mobile
        const dropdowns = document.querySelectorAll(".dropdown > a")

        dropdowns.forEach((dropdown) => {
            dropdown.addEventListener("click", function (e) {
                // Only prevent default if we're on mobile
                if (isMobile) {
                    e.preventDefault()
                    const content = this.nextElementSibling

                    // Toggle the display of the dropdown content
                    if (content.style.display === "block") {
                        content.style.display = "none"
                    } else {
                        // Hide all other dropdowns first
                        document.querySelectorAll(".dropdown-content").forEach((el) => {
                            el.style.display = "none"
                        })

                        content.style.display = "block"
                    }
                }
            })
        })
    }
})

// Add multilevel dropdown functionality for Bootstrap
document.addEventListener("DOMContentLoaded", () => {
    // First level dropdown
    const dropdownToggle = document.querySelectorAll(".dropdown-toggle")
    dropdownToggle.forEach((element) => {
        element.addEventListener("click", function (e) {
            if (window.innerWidth < 992) {
                e.preventDefault()
                e.stopPropagation()
                const dropdown = this.nextElementSibling
                if (dropdown.style.display === "block") {
                    dropdown.style.display = "none"
                } else {
                    dropdown.style.display = "block"
                }
            }
        })
    })

    // For multilevel dropdowns on desktop
    if (window.innerWidth > 991) {
        const dropdownSubmenus = document.querySelectorAll(".dropdown-submenu")
        dropdownSubmenus.forEach((element) => {
            element.addEventListener("mouseover", function () {
                this.querySelector(".dropdown-menu").style.display = "block"
            })
            element.addEventListener("mouseout", function () {
                this.querySelector(".dropdown-menu").style.display = "none"
            })
        })
    }

    // Handle submenu clicks on mobile
    const dropdownSubmenus = document.querySelectorAll(".dropdown-submenu > a")
    dropdownSubmenus.forEach((element) => {
        element.addEventListener("click", function (e) {
            if (window.innerWidth < 992) {
                e.preventDefault()
                e.stopPropagation()
                const submenu = this.nextElementSibling
                if (submenu.style.display === "block") {
                    submenu.style.display = "none"
                } else {
                    submenu.style.display = "block"
                }
            }
        })
    })
})
