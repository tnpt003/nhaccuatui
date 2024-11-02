document.getElementById('search').addEventListener('input', function () {
    const query = this.value.toLowerCase();
    const songsContainer = document.querySelector('.song-list');
    const artistsContainer = document.querySelector('.artist-list');

    songsContainer.innerHTML = '';
    artistsContainer.innerHTML = '';

    const songs = [
        { title: "Em Gái Mưa", artist: "Hương Tràm" },
        { title: "Chạy Ngay Đi", artist: "Sơn Tùng M-TP" },
        { title: "Nơi Này Có Anh", artist: "Sơn Tùng M-TP" },
        { title: "Bùa Yêu", artist: "Bích Phương" }
    ];

    const artists = [
        { name: "Hương Tràm" },
        { name: "Sơn Tùng M-TP" },
        { name: "Bích Phương" }
    ];

    const filteredSongs = songs.filter(song => song.title.toLowerCase().includes(query));
    const filteredArtists = artists.filter(artist => artist.name.toLowerCase().includes(query));

    if (filteredSongs.length > 0) {
        filteredSongs.forEach(song => {
            const songItem = document.createElement('div');
            songItem.classList.add('song-item');
            songItem.innerHTML = `${song.title} - ${song.artist}`;
            songsContainer.appendChild(songItem);
        });
        document.getElementById('songs').style.display = 'block';
    } else {
        document.getElementById('songs').style.display = 'none';
    }

    if (filteredArtists.length > 0) {
        filteredArtists.forEach(artist => {
            const artistItem = document.createElement('div');
            artistItem.classList.add('artist-item');
            artistItem.innerHTML = artist.name;
            artistsContainer.appendChild(artistItem);
        });
        document.getElementById('artists').style.display = 'block';
    } else {
        document.getElementById('artists').style.display = 'none';
    }
});
