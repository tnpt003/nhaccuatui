function truncateAuthorNames(selector) {
    const elements = document.querySelectorAll(selector);
    elements.forEach(element => {
        const fullText = element.textContent;
        element.textContent = fullText; // Reset to full text

        const containerWidth = element.clientWidth;

        // Check if the text overflows
        if (element.scrollWidth > containerWidth) {
            const parts = fullText.split(','); // Split by comma
            if (parts.length > 1) {
                const truncatedText = parts[0].trim() + ', ...'; // Truncate with ellipsis
                element.textContent = truncatedText;
            } else {
                element.textContent = fullText.substring(0, 15) + '...'; // Fallback if no comma
            }
        }
    });
}

// Call the function on page load and resize
window.addEventListener('load', () => truncateAuthorNames('.name_singer'));
window.addEventListener('resize', () => truncateAuthorNames('.name_singer'));
