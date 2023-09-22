USE SuperheroesDb;

ALTER TABLE Assistant
ADD [SuperheroId] INT NOT NULL,
CONSTRAINT FK_Superhero_Assistant FOREIGN KEY ([SuperheroId])
REFERENCES Superhero(Id);
