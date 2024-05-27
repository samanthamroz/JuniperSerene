-> REINA_HUB

=== REINA_HUB ===

NAME: *Jaia*, traveler. 
NAME: What can we do for ya?
*   [Are you... treasure hunters?]
    Reina: How dare you! We are travelling collectors — antiquarians, if you will!
    Maliya: We're treasure hunters.
    Reina: Collectors. Although... we do have quite a few treasures to trade, if you're interested.
    ** [What do you have?]
        Reina: Well, do you have a Luitaere collection?
        *** (has_collection) [Yes!]
            Reina: Fantastic!
        *** [...No?]
            Reina: Well here is your opportunity to start one!
        --- <> We recently came into possession of two beautiful, hand-painted, silver-embossed, one-of-a-kind—
            Maliya: It's a pair of Luitaere cards. The five of Dreams and seven of Stones, I believe.
            Caeran: It's the seven of Swords.
            Reina: Yes, right! Uhm... The perfect
                {has_collection: 
                    <> additions 
                - else:
                    <> start
                }
                <> to any collection! 
            Caeran: And quite a find, as well. Only 55 rings each.
            Maliya: Huh? Collectors would buy them for 90 each— 
            Reina: Let's just say 100 rings for the pair! What do you say? -> REINA_BATTLE
    ** [Oh, sorry, we're not interested.] -> DONE
    
*   [Just passing by] -> DONE

=== REINA_BATTLE ===

-> DONE