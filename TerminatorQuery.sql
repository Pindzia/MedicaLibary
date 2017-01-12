delete from parametr
DBCC CHECKIDENT ('parametr', RESEED, 0)

delete from pacjent
dbcc checkident ('pacjent', RESEED, 0)

delete from modyfikacja
dbcc checkident ('modyfikacja', RESEED, 0)

delete from wersja
dbcc checkident ('wersja', RESEED, 0)

TRUNCATE TABLE dane_modyfikacji
TRUNCATE TABLE wizyta
TRUNCATE TABLE przypisanie_parametru

TRUNCATE TABLE zasada
delete from magazyn
dbcc checkident ('magazyn', RESEED, 0)

insert into magazyn values (1, 'DomyslnyMagazyn',5000,5000)
insert into zasada values (1,1,'idp','greater','0',1)
