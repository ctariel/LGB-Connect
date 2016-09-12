# LGB-Connect
Logiciel pour les postes clients de cybergestionnaire :
http://sourceforge.net/projects/cybergestionnaire/ ou http://maxletesteur.jimdo.com/ateliers/supports-animateurs/cyber-gestionnaire/

# LGB Connect version 0.4.0

Cete version n'apporte pas de nouveauté du coté utilisateur. Elle est surtout destinée à résoudre les problèmes d'instabilité rencontrés dans les version 0.3.x. Ces instabilités étaient du au code qui interceptait les raccourcis clavier tels que ctrl+esc ou alt+f4. En cas de charge système importante, ou simplement lors de la sortie de veille, le "low level hook" était supprimé sans avertissement par Windows, entrainant le crash de l'application. Ce "Low Level Hook" est désormais géré différement, et ne devrait plus entrainer le crash en cas de dysfonctionnement (en tout cas, mes tests ont été concluants !). 

Comme pour les versions précédentes, pour mettre à jour LGB-Connect, il faut d'abord désinstaller l'ancienne version, redémarrer, puis réinstaller la nouvelle et re-redémarrer.


## LGB Connect version 0.3.0

Cette version apporte une nouveauté et quelques corrections de bugs, mais est surtout le fruit d'un travail de ré-écriture du code pour y ajouter une couche objet, et rendre les choses un peu plus "lisibles". C'est donc "sous le capot" qu'a eu lieu ma majeure partie du travail.

Commençons par la nouveauté : LGB-Connect supporte désormais les réservations. Le fonctionnement choisi est le suivant : 
- si le poste est libre, il est verrouillé 5 minutes avant le début de la réservation.
- s'il est occupé, la personne qui arrive est prévenue de la réservation, et son temps restant est fixé pour finir une minute avant le début de la réservation.
- si la personne arrive avant la réservation, on lui donne le choix d'utiliser sa réservation ou non. Néanmoins, sa réservation ne durera que le temps qu'il lui reste sur son forfait après la première utilisation.

Les bugs corrigés : 
- La base de registre est correctement réinitialisée à la fermeture du programme
- il n'est plus possible de se logguer sur 2 postes avec le même identifiant
- la date de dernière visite de l'utilisateur est désormais renseignée dans la base
- un bug subtil sur le décalage horaire entre les postes clients et le serveur qui faisait que la console de CyberGestionnaire ne "voyait" pas forcément les postes (si il y avait plus de 60 secondes d'écart, c'était mort). Désormais, c'est l'heure du serveur qui est systématiquement utilisée. Cela évite ce problème particulier.

Pour mettre à jour LGB-Connect, il faut d'abord désinstaller l'ancienne version, puis réinstaller la nouvelle. Je suis en train de chercher comment faire pour simplifier ça...

## LGB Connect version 0.2.2

Suivant les demandes du forum, 2 améliorations :
* Si l'utilisateur n'a pas de temps limite à son forfait (temps = 0), on lui affecte de manière arbitraire 24h00, et il n'y a plus d'affichage du temps restant.(Attention, ça coupe quand même au bout de 24h00 !!)
* Il y a maintenant une possibilité de choisir les chronomètres affichés en ajoutant **manuellement** une option au fichier de configuration (config.txt dans le répertoire du programme). Cette option est : "chrono" et elle peut prendre les valeurs suivante :
    * "complet" : comportement par défaut,
    * "restant" : on n'affiche que le temps restant dans la session. Attention, si en plus, le forfait est à 0, on n'affiche plus rien !,
    * "utilise" : j'ai pas mis d'accent, n'affiche plus que le temps utilisé pendant la session,
    * "aucun" : je vous laisse deviner !

Par exemple : "chrono=utilise" dans la section "[poste]" de fichier de configuration.

## LGB Connect version 0.2.1

Cette version apporte plusieurs améliorations :
* *Ajout d'un système de log :* en ajoutant manuellement une directive "debug = all" dans le fichier de configuration (config.txt) sous la section "[poste]", un fichier de log (logfile.txt) est créé dans le répertoire du programme. Ce fichier contient la grande majorité des actions effectuées par le programme, particulièrement les requêtes SQL importantes. Néanmoins, le travail est à finir, tout n'est pas encore loggué.
* *Ajout d'un formulaire de préinscription :* calqué sur CyberGestionnaire, ce formulaire nécessite encore quelques tests (je viens tout juste de le mettre en production dans notre espace...), mais il devrait fonctionner. Pour activer cette pré-inscription, il faut activer la page d'inscription dans la configuration de EPN Connect dans CyberGestionnaire. Cette configuration est indépendante de l'onglet "inscriptions" qui lui, sert au Captcha et à la préinscription VIA CyberGestionnaire. **ATTENTION !!** Ca ne corrige pas les bugs de l'interface d'administration de CyberGestionnaire, qui ne vérifie aucun champ de saisie. Vous devriez vous en rendre compte dès qu'un utilisateur mettra un apostrophe dans son nom...
* *Ajout dun décompte du temps restant :* sur une demande de mes utilisateurs, ils savent désormais combien de temps ils ont passé sur le poste, et combien de temps il leur reste.
* *correction de plusieurs bug*, dont un important sur le décompte du temps : seules les minutes étaient enregistrées, ce qui fait que si quelqu'un restait 1h05, seules 5 minutes lui étaient comptées...
* *premières verifications de cohérences sur les sessions* : sur la première version, en cas de plantage, la session ouverte le restait dans CyberGestionnaire, ce qui entrainait des temps de connexions délirants. Désormais, au lancement du logiciel, toute session ouverte (et donc "fantome") est refermée de force, et son temps est mis à zéro.


### LGB Connect version 0.1.1

Cette version devrait normalement fonctionner avec un utilisateur restreint sous Windows 10.

Les fonctionnalités sont :
- Assistant de configuration
- Blocage du poste
- Inscription des statistiques dans Cyber-Gestionnaire
- Déconnexion automatique en cas de dépassement du forfait
- Possibilité de forcer la déconnexion depuis la console

# Utilisation

Il faut commencer par installer le logiciel. Pour ça, aller faire un tour dans le section "release". Il est nécessaire de redémarrer le poste pour mettre en route le logiciel.

Au premier redémarrage, l'assistant de configuration devrait apparaitre (raccourci dans le menu "démarrage"). Une fois rempli les champs, le logiciel se lance.

Pour arrêter le logiciel, il est nécessaire de se connecter en tant qu'animateur ou administrateur, et de faire un clic droit sur l'icone à coté de l'heure dans la barre des tâches. Un menu vous propose alors de re-paramétrer l'application, ou de la quitter.

