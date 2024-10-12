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