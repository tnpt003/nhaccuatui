
function toggleFollow() {
    const followBtn = document.querySelector('.follow-btn');
    followBtn.classList.toggle('following');
    if (followBtn.classList.contains('following')) {
        followBtn.innerHTML = '<i class="fas fa-user-minus"></i> Đang theo dõi';
    } else {
        followBtn.innerHTML = '<i class="fas fa-user-plus"></i> Theo dõi';
    }
}


function openModal() {
    document.getElementById("playlistModal").style.display = "block";
}

function closeModal() {
    document.getElementById("playlistModal").style.display = "none";
}

function createPlaylist() {
    const playlistName = document.getElementById("playlistName").value;
    if (playlistName) {
        alert("Playlist '" + playlistName + "' đã được tạo!");
        closeModal();
    } else {
        alert("Vui lòng nhập tên Playlist!");
    }
}
