CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    DateJoined DATETIME DEFAULT GETDATE()
);
CREATE TABLE Artists (
    ArtistID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Bio TEXT,
    Country VARCHAR(50)
);
CREATE TABLE Albums (
    AlbumID INT IDENTITY(1,1) PRIMARY KEY,
    ArtistID INT,
    Title VARCHAR(150) NOT NULL,
    ReleaseDate DATE,
    CoverImageUrl VARCHAR(255),
    FOREIGN KEY (ArtistID) REFERENCES Artists(ArtistID)
);
CREATE TABLE Genres (
    GenreID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE
);
CREATE TABLE Songs (
    SongID INT IDENTITY(1,1) PRIMARY KEY,
    AlbumID INT,
    GenreID INT,
    Title VARCHAR(150) NOT NULL,
    Duration TIME,
    ReleaseDate DATE,
    FileUrl VARCHAR(255),
    FOREIGN KEY (AlbumID) REFERENCES Albums(AlbumID),
    FOREIGN KEY (GenreID) REFERENCES Genres(GenreID)
);
CREATE TABLE Playlists (
    PlaylistID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT,
    Name VARCHAR(100) NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
CREATE TABLE PlaylistSongs (
    PlaylistID INT,
    SongID INT,
    AddedDate DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (PlaylistID, SongID),
    FOREIGN KEY (PlaylistID) REFERENCES Playlists(PlaylistID),
    FOREIGN KEY (SongID) REFERENCES Songs(SongID)
);
CREATE TABLE Likes (
    LikeID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT,
    SongID INT,
    LikedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (SongID) REFERENCES Songs(SongID)
);
CREATE TABLE Comments (
    CommentID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT,
    SongID INT,
    CommentText TEXT,
    CommentDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (SongID) REFERENCES Songs(SongID)
);
-- Insert sample data into Users table
INSERT INTO Users (Username, Email, PasswordHash) VALUES 
('user1', 'user1@example.com', 'hashedpassword1'),
('user2', 'user2@example.com', 'hashedpassword2'),
('user3', 'user3@example.com', 'hashedpassword3');

-- Insert sample data into Artists table
INSERT INTO Artists (Name, Bio, Country) VALUES 
('Artist One', 'Bio of Artist One', 'USA'),
('Artist Two', 'Bio of Artist Two', 'UK'),
('Artist Three', 'Bio of Artist Three', 'Canada');

-- Insert sample data into Genres table
INSERT INTO Genres (Name) VALUES 
('Pop'),
('Rock'),
('Hip-Hop'),
('Jazz'),
('Classical');

-- Insert sample data into Albums table
INSERT INTO Albums (ArtistID, Title, ReleaseDate, CoverImageUrl) VALUES 
(1, 'Album One', '2024-01-15', 'http://example.com/album1.jpg'),
(2, 'Album Two', '2024-02-20', 'http://example.com/album2.jpg'),
(3, 'Album Three', '2024-03-30', 'http://example.com/album3.jpg');

-- Insert sample data into Songs table
INSERT INTO Songs (AlbumID, GenreID, Title, Duration, ReleaseDate, FileUrl) VALUES 
(1, 1, 'Song One', '00:03:30', '2024-01-15', 'http://example.com/song1.mp3'),
(1, 2, 'Song Two', '00:04:00', '2024-01-15', 'http://example.com/song2.mp3'),
(2, 3, 'Song Three', '00:02:45', '2024-02-20', 'http://example.com/song3.mp3'),
(3, 4, 'Song Four', '00:05:10', '2024-03-30', 'http://example.com/song4.mp3');

-- Insert sample data into Playlists table
INSERT INTO Playlists (UserID, Name) VALUES 
(1, 'My Favorite Songs'),
(2, 'Chill Vibes'),
(3, 'Workout Playlist');

-- Insert sample data into PlaylistSongs table
INSERT INTO PlaylistSongs (PlaylistID, SongID) VALUES 
(1, 1),
(1, 2),
(2, 3),
(3, 4);

-- Insert sample data into Likes table
INSERT INTO Likes (UserID, SongID) VALUES 
(1, 1),
(2, 2),
(1, 3),
(3, 4);

-- Insert sample data into Comments table
INSERT INTO Comments (UserID, SongID, CommentText) VALUES 
(1, 1, 'Great song!'),
(2, 2, 'I love this track!'),
(3, 3, 'This one is awesome!'),
(1, 4, 'Not my favorite.');

ALTER TABLE Songs
ADD ImageUrl VARCHAR(255);
INSERT INTO Songs (AlbumID, GenreID, Title, Duration, ReleaseDate, FileUrl, ImageUrl) VALUES 
(1, 1, 'Song One', '00:03:30', '2024-01-15', 'http://example.com/song1.mp3', 'Song One.jpg');
select * from Songs

SELECT TOP 5 * FROM Albums ORDER BY NEWID()

SELECT TOP 1 
    s.SongID, 
    s.Title, 
    s.FileUrl, 
    s.AlbumID, 
    s.ReleaseDate,
    a.Name AS ArtistName, 
    COUNT(l.LikeID) AS LikeCount 
FROM Songs s
LEFT JOIN Likes l ON s.SongID = l.SongID
LEFT JOIN Artists a ON a.ArtistID = (SELECT ArtistID FROM Albums WHERE AlbumID = s.AlbumID)
GROUP BY s.SongID, s.Title, s.FileUrl, s.AlbumID, s.ReleaseDate, a.Name
ORDER BY LikeCount DESC

SELECT 
    s.SongID, 
    s.Title, 
    s.FileUrl, 
    s.AlbumID, 
    s.ReleaseDate,
    a.Name AS ArtistName, 
    COUNT(l.LikeID) AS LikeCount 
FROM Songs s
LEFT JOIN Likes l ON s.SongID = l.SongID
LEFT JOIN Artists a ON a.ArtistID = (SELECT ArtistID FROM Albums WHERE AlbumID = s.AlbumID)
GROUP BY s.SongID, s.Title, s.FileUrl, s.AlbumID, s.ReleaseDate, a.Name
ORDER BY LikeCount DESC
OFFSET 1 ROWS FETCH NEXT 9 ROWS ONLY;

select * from Albums
SELECT 
    s.SongID, 
    s.Title, 
    s.FileUrl, 
    s.AlbumID, 
    s.ReleaseDate,
    a.Name AS ArtistName, 
    COUNT(l.LikeID) AS LikeCount 
FROM Songs s
LEFT JOIN Likes l ON s.SongID = l.SongID
LEFT JOIN Artists a ON a.ArtistID = (SELECT ArtistID FROM Albums WHERE AlbumID = s.AlbumID)
GROUP BY s.SongID, s.Title, s.FileUrl, s.AlbumID, s.ReleaseDate, a.Name
ORDER BY LikeCount DESC
OFFSET 1 ROWS FETCH NEXT 9 ROWS ONLY

