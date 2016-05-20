# LGB-Connect
Logiciel pour les postes clients de cybergestionnaire :
http://sourceforge.net/projects/cybergestionnaire/ ou http://maxletesteur.jimdo.com/ateliers/supports-animateurs/cyber-gestionnaire/

# LGB Connect version 0.2.1

Cette version apporte plusieurs améliorations :
- *Ajout d'un système de log :* en ajoutant manuellement une directive "debug = all" dans le fichier de configuration sous la section "[poste]", un fichier de log (logfile.txt) est créé dans le répertoire du programme. Ce fichier contient la grande majorité des actions effectuées par le programme, particulièrement les requêtes SQL importantes. Néanmoins, le travail est à finir, tout n'est pas encore loggué.
- *Ajout d'un formulaire de préinscription :* calqué sur CyberGestionnaire, ce formulaire nécessite encore quelques tests (je viens tout juste de le mettre en production dans notre espace...), mais il devrait fonctionner. Pour activer cette pré-inscription, il faut activer la page d'inscription dans la configuration de EPN Connect dans CyberGestionnaire. Cette configuration est indépendante de l'onglet "inscriptions" qui lui, sert au Captcha et à la préinscription VIA CyberGestionnaire. **ATTENTION !!** Ca ne corrige pas les bugs de l'interface d'administration de CyberGestionnaire, qui ne vérifie aucun champ de saisie. Vous devriez vous en rendre compte dès qu'un utilisateur mettra un apostrophe dans son nom...
- *Ajout dun décompte du temps restant :* sur une demande de mes utilisateurs, ils savent désormais combien de temps ils ont passé sur le poste, et combien de temps il leur reste.
- *correction de plusieurs bug*, dont un important sur le décompte du temps : seules les minutes étaient enregistrées, ce qui fait que si quelqu'un restait 1h05, seules 5 minutes lui étaient comptées...
- *premières verifications de cohérences sur les sessions* : sur la première version, en cas de plantage, la session ouverte le restait dans CyberGestionnaire, ce qui entrainait des temps de connexions délirants. Désormais, au lancement du logiciel, toute session ouverte (et donc "fantome") est refermée de force, et son temps est mis à zéro.


## LGB Connect version 0.1.1

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

