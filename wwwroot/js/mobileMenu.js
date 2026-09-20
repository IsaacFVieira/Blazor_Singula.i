window.singularMobileMenu = {
    isOpen: false,

    open: function() {
        this.isOpen = true;
        document.body.style.overflow = 'hidden';
        document.body.style.position = 'fixed';
        document.body.style.width = '100%';
        document.body.style.top = `-${window.scrollY}px`;
        
        const overlay = document.querySelector('.mobile-menu-overlay');
        const menu = document.querySelector('.mobile-menu');
        
        if (overlay) overlay.classList.add('mobile-menu-overlay--open');
        if (menu) menu.classList.add('mobile-menu--open');
    },

    close: function() {
        const scrollY = document.body.style.top;
        this.isOpen = false;
        document.body.style.position = '';
        document.body.style.width = '';
        document.body.style.overflow = '';
        document.body.style.top = '';
        window.scrollTo(0, parseInt(scrollY || '0') * -1);
        
        const overlay = document.querySelector('.mobile-menu-overlay');
        const menu = document.querySelector('.mobile-menu');
        
        if (overlay) overlay.classList.remove('mobile-menu-overlay--open');
        if (menu) menu.classList.remove('mobile-menu--open');
    },

    toggle: function() {
        if (this.isOpen) {
            this.close();
        } else {
            this.open();
        }
    },

    init: function() {
        // Close menu when clicking overlay
        const overlay = document.querySelector('.mobile-menu-overlay');
        if (overlay) {
            overlay.addEventListener('click', () => this.close());
        }

        // Close menu when clicking links
        const links = document.querySelectorAll('.mobile-menu-link');
        links.forEach(link => {
            link.addEventListener('click', () => this.close());
        });
    }
};
