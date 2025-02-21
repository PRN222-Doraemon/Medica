// Global counters for generating unique IDs
let chapterCounter = 0;
let resourceCounters = {};

function addChapter() {
    // Get the template and container
    const template = document.getElementById('chapterTemplate');
    const container = document.getElementById('chaptersContainer');

    // Create new chapter index
    const chapterIndex = container.children.length;
    chapterCounter++;

    // Clone the template
    const clone = template.content.cloneNode(true);

    // Replace placeholders
    const chapterHtml = clone.firstElementChild.outerHTML
        .replace(/__CHAPTER_INDEX__/g, chapterIndex)
        .replace(/__CHAPTER_ID__/g, `new-${chapterCounter}`);

    // Create element from HTML string
    const newChapter = document.createElement('div');
    newChapter.innerHTML = chapterHtml;
    const chapterElement = newChapter.firstElementChild;

    // Initialize resource counter for this chapter
    resourceCounters[chapterIndex] = 0;

    // Add to container
    container.appendChild(chapterElement);

    // Update all chapter indexes
    updateChapterIndexes();
}

function removeChapter(button) {
    const chapter = button.closest('.chapter-card');
    if (confirm('Are you sure you want to delete this chapter and all its resources?')) {
        chapter.remove();
        updateChapterIndexes();
    }
}

function addResource(button) {
    // Get the template
    const template = document.getElementById('resourceTemplate');

    // Find the chapter and its resources container
    const chapter = button.closest('.chapter-card');
    const resourcesContainer = chapter.querySelector('.resources-container');

    // Get chapter index
    const chapterIndex = chapter.dataset.chapterIndex;

    // Get resource index
    const resourceIndex = resourcesContainer.children.length;

    // Clone the template
    const clone = template.content.cloneNode(true);

    // Replace placeholders
    const resourceHtml = clone.firstElementChild.outerHTML
        .replace(/__CHAPTER_INDEX__/g, chapterIndex)
        .replace(/__RESOURCE_INDEX__/g, resourceIndex);

    // Create element from HTML string
    const newResource = document.createElement('div');
    newResource.innerHTML = resourceHtml;
    const resourceElement = newResource.firstElementChild;

    // Add to container
    resourcesContainer.appendChild(resourceElement);

    // Update all resource indexes in this chapter
    updateResourceIndexes(chapter);
}

function removeResource(button) {
    const resource = button.closest('.resource-item');
    const chapter = button.closest('.chapter-card');

    if (confirm('Are you sure you want to delete this resource?')) {
        resource.remove();
        updateResourceIndexes(chapter);
    }
}

function updateChapterIndexes() {
    const chapters = document.querySelectorAll('.chapter-card');
    chapters.forEach((chapter, index) => {
        // Update data attribute
        chapter.dataset.chapterIndex = index;

        // Update input names
        chapter.querySelectorAll('input, select, textarea').forEach(input => {
            input.name = input.name.replace(/CourseChapters\[\d+\]/, `CourseChapters[${index}]`);
        });

        // Update resource indexes
        updateResourceIndexes(chapter);
    });
}

function updateResourceIndexes(chapter) {
    const resources = chapter.querySelectorAll('.resource-item');
    const chapterIndex = chapter.dataset.chapterIndex;

    resources.forEach((resource, index) => {
        // Update data attribute
        resource.dataset.resourceIndex = index;

        // Update input names
        resource.querySelectorAll('input, select, textarea').forEach(input => {
            input.name = input.name.replace(
                /CourseChapters\[\d+\]\.Resources\[\d+\]/,
                `CourseChapters[${chapterIndex}].Resources[${index}]`
            );
        });
    });
}


// Add validation for the form
document.getElementById('updateCourseForm').addEventListener('submit', function (e) {
    const chapters = document.querySelectorAll('.chapter-card');

    // Check if there are any chapters
    if (chapters.length === 0) {
        e.preventDefault();
        alert('Please add at least one chapter to the course.');
        return;
    }

    // Check if all chapters have names
    let hasEmptyChapter = false;
    chapters.forEach(chapter => {
        const chapterName = chapter.querySelector('.chapter-name').value.trim();
        if (chapterName === '') {
            hasEmptyChapter = true;
        }
    });

    if (hasEmptyChapter) {
        e.preventDefault();
        alert('Please provide names for all chapters.');
        return;
    }
});