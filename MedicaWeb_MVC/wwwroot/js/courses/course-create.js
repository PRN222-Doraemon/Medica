let chapterCounter = 0;

async function addChapter() {
    const chaptersContainer = document.getElementById('chaptersContainer');
    const response = await fetch(`/Courses/GetChapterComponent?index=${chapterCounter}`);
    const html = await response.text();
    chaptersContainer.insertAdjacentHTML('beforeend', html);
    chapterCounter++;
}

async function addResource(button) {
    const chapter = button.closest('.chapter-card');
    const resourcesContainer = chapter.querySelector('.resources-container');
    const chapterIndex = chapter.dataset.chapterIndex;
    const resourceIndex = resourcesContainer.children.length;

    const response = await fetch(`/Courses/GetResourceComponent?chapterIndex=${chapterIndex}&resourceIndex=${resourceIndex}`);
    const html = await response.text();
    resourcesContainer.insertAdjacentHTML('beforeend', html);
}

function removeChapter(button) {
    const chapter = button.closest('.chapter-card');
    chapter.remove();
    updateChapterIndices();
}

function removeResource(button) {
    const resource = button.closest('.resource-item');
    resource.remove();
    updateResourceIndices(resource.closest('.chapter-card'));
}

function updateChapterIndices() {
    const chapters = document.querySelectorAll('.chapter-card');
    chapters.forEach((chapter, index) => {
        chapter.dataset.chapterIndex = index;
        const inputs = chapter.querySelectorAll('input[name*="CourseChapters"]');
        inputs.forEach(input => {
            input.name = input.name.replace(/CourseChapters\[\d+\]/, `CourseChapters[${index}]`);
        });
    });
}
function updateResourceIndices(chapter) {
    const resources = chapter.querySelectorAll('.resource-item');
    const chapterIndex = chapter.dataset.chapterIndex;
    resources.forEach((resource, index) => {
        resource.dataset.resourceIndex = index;
        const inputs = resource.querySelectorAll('input[name*="Resources"]');
        inputs.forEach(input => {
            input.name = input.name.replace(/Resources\[\d+\]/, `Resources[${index}]`);
        });
    });
}