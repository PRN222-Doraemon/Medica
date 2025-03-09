async function addClassToCart(classRoomId) {
    await fetch("/Cart/AddToCart", {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ classRoomId: parseInt(classRoomId) })
    })
        .then(res => res.json())
        .then(data => {
            if (data.IsSuccess) {
                alert(data.message); // Show success message
            } else {
                alert(data.message); // Show error message
            }
        })
        .catch(error => {
            alert('An error occurred while adding to cart: ' + error.message);
        });
}