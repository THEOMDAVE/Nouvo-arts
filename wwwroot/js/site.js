// Nouvo Studio JavaScript

$(document).ready(function() {
    // Initialize
    initializeApp();
});

function initializeApp() {
    setTimeout(function () { test(); });
    // Set current year
    $('#year').text(new Date().getFullYear());
    
    // Initialize smooth scrolling
    initializeSmoothScrolling();
    
    // Initialize favorites
    initializeFavorites();
    
    // Initialize animations
    initializeAnimations();
    
    // Initialize contact form
    initializeContactForm();
    
    // Initialize masonry layout
    initializeMasonry();
    
    // Initialize admin sidebar
    initializeAdminSidebar();
}

// Smooth scrolling for anchor links
function initializeSmoothScrolling() {
    $('.smooth-anchor').on('click', function(e) {
        e.preventDefault();
        const target = $(this.getAttribute('href'));
        if (target.length) {
            $('html, body').animate({
                scrollTop: target.offset().top - 80
            }, 800);
        }
    });
}

// Favorites functionality
function initializeFavorites() {
    // Load favorites from localStorage
    function getFavorites() {
        try {
            const parsed = JSON.parse(localStorage.getItem('nouvo:favorites') || '[]');
            return Array.isArray(parsed) ? parsed.map(n => Number(n)).filter(n => !Number.isNaN(n)) : [];
        } catch (e) {
            return [];
        }
    }
    
    function setFavorites(favorites) {
        localStorage.setItem('nouvo:favorites', JSON.stringify(favorites));
    }
    
    function isFavorite(id) {
        const numericId = Number(id);
        return getFavorites().includes(numericId);
    }
    
    function toggleFavorite(id) {
        const numericId = Number(id);
        const favorites = getFavorites();
        const index = favorites.indexOf(numericId);
        
        if (index > -1) {
            favorites.splice(index, 1);
        } else {
            favorites.push(numericId);
        }
        
        setFavorites(favorites);
        return favorites.includes(numericId);
    }
    
    // Update favorite buttons
    function updateFavoriteButtons() {
        $('[data-fav-id]').each(function() {
            const id = Number($(this).data('fav-id'));
            const isFav = isFavorite(id);
            const icon = $(this).find('i');
            
            if (isFav) {
                icon.removeClass('text-dark').addClass('text-danger');
            } else {
                icon.removeClass('text-danger').addClass('text-dark');
            }
        });
    }
    
    // Toggle favorite on click
    $('[data-fav-id]').on('click', function(e) {
        e.preventDefault();
        const id = Number($(this).data('fav-id'));
        const isFav = toggleFavorite(id);
        const icon = $(this).find('i');
        
        if (isFav) {
            icon.removeClass('text-dark').addClass('text-danger');
        } else {
            icon.removeClass('text-danger').addClass('text-dark');
        }
        
        // Show feedback
        showToast(isFav ? 'Added to favorites' : 'Removed from favorites');
    });
    
    // Initialize favorite buttons
    updateFavoriteButtons();
}

// Animation on scroll
function initializeAnimations() {
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('fade-up');
            }
        });
    }, { threshold: 0.2 });
    
    document.querySelectorAll('.fade-up').forEach(el => {
        observer.observe(el);
    });
}

// Contact form
function initializeContactForm() {
    $('#contactForm').on('submit', function(e) {
        e.preventDefault();
        
        const form = this;
        form.classList.add('was-validated');
        
        if (!form.checkValidity()) {
            return;
        }
        
        // Simulate form submission
        setTimeout(() => {
            $('#contactSuccess').removeClass('d-none');
            form.reset();
            form.classList.remove('was-validated');
        }, 500);
    });
}

// Toast notifications
function showToast(message, type = 'info') {
    const toast = $(`
        <div class="toast align-items-center text-white bg-${type} border-0" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="d-flex">
                <div class="toast-body">${message}</div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
            </div>
        </div>
    `);
    
    $('body').append(toast);
    
    const bsToast = new bootstrap.Toast(toast[0]);
    bsToast.show();
    
    // Remove toast element after it's hidden
    toast.on('hidden.bs.toast', function() {
        $(this).remove();
    });
}

// Search functionality
function initializeSearch() {
    $('#searchInput').on('input', function() {
        const query = $(this).val().toLowerCase();
        filterArtworks(query);
    });
}

function filterArtworks(query) {
    $('.art-card').each(function() {
        const $card = $(this);
        const title = $card.find('.fw-semibold').text().toLowerCase();
        const code = $card.find('.text-muted.small').text().toLowerCase();
        
        if (title.includes(query) || code.includes(query)) {
            $card.parent().show();
        } else {
            $card.parent().hide();
        }
    });
}

// Filter functionality
function initializeFilters() {
    $('#sizeFilter, #mediumFilter').on('change', function() {
        applyFilters();
    });
}

function applyFilters() {
    const sizeFilter = $('#sizeFilter').val();
    const mediumFilter = $('#mediumFilter').val();
    
    $('.art-card').each(function() {
        const $card = $(this);
        const size = $card.find('.small.text-muted').text().split(' • ')[0];
        const medium = $card.find('.small.text-muted').text().split(' • ')[1];
        
        let show = true;
        
        if (sizeFilter && size !== sizeFilter) {
            show = false;
        }
        
        if (mediumFilter && medium !== mediumFilter) {
            show = false;
        }
        
        if (show) {
            $card.parent().show();
        } else {
            $card.parent().hide();
        }
    });
}

// Price toggle
function initializePriceToggle() {
    $('#priceToggle').on('change', function() {
        const show = $(this).is(':checked');
        $('.price').toggle(show);
    });
}

// Initialize all functionality when document is ready
$(document).ready(function() {
    initializeSearch();
    initializeFilters();
    initializePriceToggle();
});

// API Service for AJAX calls
class ApiService {
    constructor(baseUrl = '/api') {
        this.baseUrl = baseUrl;
    }

    async request(endpoint, options = {}) {
        const url = `${this.baseUrl}${endpoint}`;
        const config = {
            headers: {
                'Content-Type': 'application/json',
                ...options.headers
            },
            ...options
        };

        try {
            const response = await fetch(url, config);
            
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            
            return await response.json();
        } catch (error) {
            console.error('API request failed:', error);
            throw error;
        }
    }

    async getCategories() {
        return this.request('/categories');
    }

    async getCategory(id) {
        return this.request(`/categories/${id}`);
    }

    async createCategory(categoryData) {
        return this.request('/categories', {
            method: 'POST',
            body: JSON.stringify(categoryData)
        });
    }

    async updateCategory(id, categoryData) {
        return this.request(`/categories/${id}`, {
            method: 'PUT',
            body: JSON.stringify(categoryData)
        });
    }

    async deleteCategory(id) {
        return this.request(`/categories/${id}`, {
            method: 'DELETE'
        });
    }

    async getArtworks(filters = {}) {
        const queryParams = new URLSearchParams(filters);
        const endpoint = queryParams.toString() ? `/artworks?${queryParams}` : '/artworks';
        return this.request(endpoint);
    }

    async getArtwork(id) {
        return this.request(`/artworks/${id}`);
    }

    async createArtwork(artworkData) {
        return this.request('/artworks', {
            method: 'POST',
            body: JSON.stringify(artworkData)
        });
    }

    async updateArtwork(id, artworkData) {
        return this.request(`/artworks/${id}`, {
            method: 'PUT',
            body: JSON.stringify(artworkData)
        });
    }

    async deleteArtwork(id) {
        return this.request(`/artworks/${id}`, {
            method: 'DELETE'
        });
    }

    async searchArtworks(query, filters = {}) {
        return this.request('/artworks/search', {
            method: 'POST',
            body: JSON.stringify({ query, ...filters })
        });
    }
}

// Create global API service instance
window.apiService = new ApiService();

// Masonry Layout Functionality
function initializeMasonry() {
    // Wait for images to load before initializing masonry
    const masonryContainer = document.querySelector('.masonry-container');
    if (!masonryContainer) return;
    
    // Function to initialize masonry after images load
    function initMasonryAfterImagesLoad() {
        const images = masonryContainer.querySelectorAll('img');
        let loadedImages = 0;
        
        if (images.length === 0) {
            // No images, initialize immediately
            adjustMasonryLayout();
            return;
        }
        
        images.forEach(img => {
            if (img.complete) {
                loadedImages++;
                if (loadedImages === images.length) {
                    adjustMasonryLayout();
                }
            } else {
                img.addEventListener('load', () => {
                    loadedImages++;
                    if (loadedImages === images.length) {
                        adjustMasonryLayout();
                    }
                });
            }
        });
    }
    
    // Function to adjust masonry layout
    function adjustMasonryLayout() {
        // Add a small delay to ensure DOM is fully rendered
        setTimeout(() => {
            const items = masonryContainer.querySelectorAll('.masonry-item');
            items.forEach(item => {
                // Reset any inline styles that might interfere
                item.style.breakInside = 'avoid';
                item.style.pageBreakInside = 'avoid';
            });
        }, 100);
    }
    
    // Initialize masonry
    initMasonryAfterImagesLoad();
    
    // Re-initialize on window resize
    let resizeTimeout;
    window.addEventListener('resize', () => {
        clearTimeout(resizeTimeout);
        resizeTimeout = setTimeout(adjustMasonryLayout, 250);
    });
    
    // Re-initialize when new content is loaded (for filters, etc.)
    const observer = new MutationObserver((mutations) => {
        mutations.forEach((mutation) => {
            if (mutation.type === 'childList' && mutation.target === masonryContainer) {
                initMasonryAfterImagesLoad();
            }
        });
    });
    
    observer.observe(masonryContainer, {
        childList: true,
        subtree: true
    });
}
function test() {
    var $nav = $('#navbarSupportedContent');
    var $hori = $('.hori-selector');

    // 👉 EXIT if navbar not present
    if ($nav.length === 0 || $hori.length === 0) return;

    var lastActiveHref = localStorage.getItem('activeNavHref');

    var path = window.location.pathname.split("/").pop();
    if (path === '' || path === undefined) path = 'index';

    var $target = $nav.find('ul li a[href$="' + path + '"]').parent();

    if ($target.length === 0 && lastActiveHref) {
        $target = $nav.find('ul li a[href="' + lastActiveHref + '"]').parent();
    }

    if ($target.length === 0) {
        $target = $nav.find('ul li:first');
    }

    $nav.find('ul li').removeClass('active');
    $target.addClass('active');

    moveSelector($target, false);

    $nav.off('click').on('click', 'li', function () {

        var $this = $(this);

        $nav.find('ul li').removeClass('active');
        $this.addClass('active');

        var href = $this.find('a').attr('href');
        localStorage.setItem('activeNavHref', href);

        moveSelector($this, true);
    });

    $(window).on('resize', function () {
        var $activeItem = $nav.find('ul li.active');
        if ($activeItem.length) moveSelector($activeItem, false);
    });

    function moveSelector($el, animate = true) {

        if (!$el || !$el.length) return;   // 👈 FINAL PROTECTION

        var itemPos = $el.position();
        if (!itemPos) return;

        var activeHeight = $el.innerHeight();
        var activeWidth = $el.innerWidth();

        const cssObj = {
            top: itemPos.top + "px",
            left: itemPos.left + "px",
            height: activeHeight + "px",
            width: activeWidth + "px"
        };

        if (animate) {
            $hori.stop().animate(cssObj, 350);
        } else {
            $hori.css(cssObj);
        }
    }
}


//function test() {
//    var $nav = $('#navbarSupportedContent');
//    var $hori = $('.hori-selector');

//    // Retrieve last clicked item from localStorage
//    var lastActiveHref = localStorage.getItem('activeNavHref');

//    // Determine current path (for current page)
//    var path = window.location.pathname.split("/").pop();
//    if (path === '' || path === undefined) path = 'index';

//    // Find matching <a> link for current page
//    var $target = $nav.find('ul li a[href$="' + path + '"]').parent();

//    // If no match, use last active link from storage
//    if ($target.length === 0 && lastActiveHref) {
//        $target = $nav.find('ul li a[href="' + lastActiveHref + '"]').parent();
//    }

//    // If still nothing, default to first li
//    if ($target.length === 0) {
//        $target = $nav.find('ul li:first');
//    }

//    // Remove previous actives and mark the right one
//    $nav.find('ul li').removeClass('active');
//    $target.addClass('active');

//    // Instantly set selector position to last known position (no jump)
//    moveSelector($target, false);

//    // Store new active on click and animate
//    $nav.off('click').on('click', 'li', function () {
//        var $this = $(this);
//        $nav.find('ul li').removeClass('active');
//        $this.addClass('active');

//        // Save clicked link href (so we remember after reload)
//        var href = $this.find('a').attr('href');
//        localStorage.setItem('activeNavHref', href);

//        // Animate from current to new position
//        moveSelector($this, true);
//    });

//    // Keep it correct on resize
//    $(window).on('resize', function () {
//        var $activeItem = $nav.find('ul li.active');
//        moveSelector($activeItem, false);
//    });

//    // Function to move the selector
//    function moveSelector($el, animate = true) {
//        var itemPos = $el.position();
//        var activeHeight = $el.innerHeight();
//        var activeWidth = $el.innerWidth();

//        if (animate) {
//            $hori.stop().animate({
//                top: itemPos.top + "px",
//                left: itemPos.left + "px",
//                height: activeHeight + "px",
//                width: activeWidth + "px"
//            }, 350);
//        } else {
//            $hori.css({
//                top: itemPos.top + "px",
//                left: itemPos.left + "px",
//                height: activeHeight + "px",
//                width: activeWidth + "px"
//            });
//        }
//    }
//}

// Admin Sidebar Toggle Functionality
function initializeAdminSidebar() {
    const $sidebar = $('#adminSidebar');
    const $overlay = $('#adminSidebarOverlay');
    const $toggle = $('#adminSidebarToggle');
    const $close = $('#adminSidebarClose');
    const $body = $('body');
    
    // Only initialize if sidebar exists
    if ($sidebar.length === 0) return;
    
    // Toggle sidebar open
    function openSidebar() {
        $sidebar.addClass('show');
        $overlay.addClass('show');
        $body.addClass('admin-sidebar-open');
    }
    
    // Toggle sidebar closed
    function closeSidebar() {
        $sidebar.removeClass('show');
        $overlay.removeClass('show');
        $body.removeClass('admin-sidebar-open');
    }
    
    // Toggle button click
    if ($toggle.length > 0) {
        $toggle.on('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            if ($sidebar.hasClass('show')) {
                closeSidebar();
            } else {
                openSidebar();
            }
        });
    }
    
    // Close button click
    if ($close.length > 0) {
        $close.on('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            closeSidebar();
        });
    }
    
    // Overlay click to close
    if ($overlay.length > 0) {
        $overlay.on('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            closeSidebar();
        });
    }
    
    // Close on escape key
    $(document).on('keydown', function(e) {
        if (e.key === 'Escape' && $sidebar.hasClass('show')) {
            closeSidebar();
        }
    });
    
    // Close sidebar when clicking on a nav link (mobile only)
    $sidebar.find('.nav-link').on('click', function() {
        if (window.innerWidth < 768) {
            setTimeout(closeSidebar, 300);
        }
    });
    
    // Handle window resize
    let resizeTimer;
    $(window).on('resize', function() {
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(function() {
            if (window.innerWidth >= 768) {
                closeSidebar();
            }
        }, 250);
    });
    
    // Set active nav link based on current page
    setActiveNavLink();
}

// Set active navigation link based on current URL
function setActiveNavLink() {
    const $sidebar = $('#adminSidebar');
    if ($sidebar.length === 0) return;
    
    const currentPath = window.location.pathname.toLowerCase();
    const $navLinks = $sidebar.find('.nav-link');
    
    $navLinks.removeClass('active');
    
    $navLinks.each(function() {
        const $link = $(this);
        const href = $link.attr('href');
        
        if (href) {
            const linkPath = href.toLowerCase();
            // Check if current path matches the link path
            if (currentPath === linkPath || 
                (linkPath !== '/' && currentPath.startsWith(linkPath))) {
                $link.addClass('active');
                return false; // break loop
            }
        }
    });
}

function autoFilter() {
    document.getElementById("filterForm").submit();
}

//function test() {
//    if ($('#navbarSupportedContent ul li').hasClass("active").length <= 0) {
//        $('#navbarSupportedContent ul li:nthclass').addClass('active');
//    }
//    var tabsNewAnim = $('#navbarSupportedContent');
//    var activeItemNewAnim = tabsNewAnim.find('.active');
//    var activeHeight = activeItemNewAnim.innerHeight();
//    var activeWidth = activeItemNewAnim.innerWidth();
//    var itemPos = activeItemNewAnim.position();

//    $(".hori-selector").css({
//        "top": itemPos.top + "px",
//        "left": itemPos.left + "px",
//        "height": activeHeight + "px",
//        "width": activeWidth + "px"
//    });

//    $("#navbarSupportedContent").on("click", "li", function () {
//        debugger
//        $('#navbarSupportedContent ul li').removeClass("active");
//        $(this).addClass('active');

//        var activeHeight = $(this).innerHeight();
//        var activeWidth = $(this).innerWidth();
//        var itemPos = $(this).position();

//        $(".hori-selector").css({
//            "top": itemPos.top + "px",
//            "left": itemPos.left + "px",
//            "height": activeHeight + "px",
//            "width": activeWidth + "px"
//        });
//    });
//}

//$(document).ready(function () {
//    setTimeout(function () { test(); });
//});

