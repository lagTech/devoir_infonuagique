Nom: Ulrich Lacmago Mbonwo
Code Permanent: LACU01010000

Ces modifications ont permis de rendre l'application compatible avec le stockage NoSQL de Cosmos DB

1. ConnectionString ;
La connectionString provient de l'instance locale de Cosmos DB (émulateur).

2. Modifications dans les classes d'entités;
- Post.cs :
Utilisation de Guid pour générer un identifiant unique pour les champs Id.
Ajustement de la structure de l'entité pour fonctionner avec le schéma NoSQL de Cosmos DB.
Ajout d'une collection ICollection<Comment> pour refléter les relations entre les entités.
- Comment.cs :
Modification de la structure pour utiliser string comme type d'identifiant (Id et PostId).

- Modifications des méthodes dans les contrôleurs ;
Toutes les méthodes manipulant les identifiants (Id ou PostId) ont été mises à jour pour accepter un type string au lieu de int

3. Mise à jour du fichier Program.cs
- Configuration de l'application pour utiliser Cosmos DB avec UseCosmos().
- Ajout de la méthode EnsureCreatedAsync() pour garantir que les conteneurs Cosmos DB soient créés automatiquement au démarrage de l'application.

4. Modifications dans la classe ApplicationDbContext
- Ajout de configurations spécifiques pour mapper les entités avec Cosmos DB :
- Utilisation de .ToContainer("Posts") et .ToContainer("Comments") pour associer les entités Post et Comment à leurs conteneurs respectifs.

5. Réécriture des requêtes pour Cosmos DB
- Les requêtes LINQ ont été adaptées en raison des limitations de Cosmos DB :
- Les agrégations comme .GroupBy() ont été déplacées vers un traitement côté client après l'exécution de .ToListAsync().
- Suppression des instructions .Include, car elles ne sont pas supportées par Cosmos DB.

6. Gestion des opérations asynchrones
- Cosmos DB ne prend pas en charge les opérations synchrones. Toutes les opérations de base de données ont été mises à jour pour être asynchrones :
Find -> FindAsync
ToList -> ToListAsync

7. Suppression des limitations de Cosmos DB
- Les relations entre entités (comme les commentaires d'un post) ne peuvent pas être récupérées via .Include. À la place :
- Les relations sont récupérées séparément, et les données sont fusionnées manuellement au besoin.

8. Ajout de gestion pour les limitations de LINQ dans Cosmos DB
- Les requêtes complexes (comme .GroupBy) ont été adaptées pour être exécutées côté client après la récupération des données.