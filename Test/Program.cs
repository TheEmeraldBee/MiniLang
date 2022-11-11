﻿using MiniLang.Core;
using MiniLang.Internal;

var engine = new Engine(new DefaultWriter());
engine.AddLibrary(new DefaultLibrary());

var codeText = @"
$26 *90 Z
$25 *89 Y
$24 *88 X
$23 *87 W
$22 *86 V
$21 *85 U
$20 *84 T
$19 *83 S
$18 *82 R
$17 *81 Q
$16 *80 P
$15 *79 O
$14 *78 N
$13 *77 M
$12 *76 L
$11 *75 K
$10 *74 J
$9 *73 I
$8 *72 H
$7 *71 G
$6 *70 F
$5 *69 E
$4 *68 D
$3 *67 C
$2 *66 B
$1 *65 A
$0 *32 SPACE

$90 *5 { # Tries }

{ # Needed Numbers For Display }
$98 *0 
$99 *100

{ Guess Number }
$100 ?100/

{Exit Code}
$89*0

{Game Loop} 
$90 [

{ Guess }
$7^ $21^ $5^ $19^ $19^ / $98. $0^^ $99.  /
$101 _

{ If Greater }
($101 => $100 | $12^/)

{ If Less }
($101 =< $100 | $7^/)

{ If Equal Exit Game and set exit code }
($101 = $100 | $23^/ $89*1)

$90-
($89 = $666*1 | $90*0)

$90
]

{ If Exit Code Is 0, Loss }
($89 = $666*0 | $12^)

";

engine.Run(codeText);

