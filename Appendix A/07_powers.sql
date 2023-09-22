USE SuperheroesDb;

INSERT INTO [Power] ([Name], [Description])
VALUES
('Tree rotations', 'Rotates trees at an incredible speed.'),
('Water moving', 'Moves water at a moderate speed.'),
('Nynorsk translation', 'Translates nynorsk at an alarming speed.'),
('Shoe dryer', 'Ability to dry shoes at a normal speed by intense manifesting.');

INSERT INTO [SuperheroPowerLink] ([SuperheroId], [PowerId])
VALUES
(1, 1),
(1, 2),
(2, 2),
(3, 3),
(3, 4);