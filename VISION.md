# Visión del juego

> Documento vivo. Lo mantiene **Francisco** (@fran-giordano): cualquier cambio acá necesita su aprobación.
> Si una idea contradice este documento, primero se discute la visión y después se hace la tarea.

## En una frase
Tower defense **incremental** en 3D: cada partida termina en derrota, pero cada derrota te hace más fuerte.

## Pilares
1. **Progreso constante:** cada partida deja algo permanente, aunque haya ido mal.
2. **Estrategia de ubicación:** importa dónde ponés cada torre, no solo cuánto la mejorás.
3. **Escala:** con el tiempo hay muchísimas cosas en pantalla y números grandes, y tiene que seguir leyéndose bien.
4. **Hacia el idle:** a largo plazo el juego avanza solo y el jugador toma decisiones de más alto nivel.

## Loop principal
```
Partida ─► olas infinitas, cada vez más difíciles
   │        oro (moneda de partida) → construir y mejorar torres
   ▼
Derrota ─► gemas (moneda permanente): 1 cada 5 olas + bonus por récord
   │
   ▼
Taller ──► mejoras permanentes: slots, torres nuevas, daño, oro inicial…
   │
   └──────► nueva partida, más lejos
```

## Inicio del juego
- Mapa con un camino y **1 solo slot** disponible. Torre básica.
- La **primera partida está pensada para perderse**: en la ola 5 aparece un jefe imposible.
- Al perder se cobran las primeras gemas y se abre el taller con 3 mejoras: **+1 slot**, **+10 % daño** y **desbloquear una torre**.
- Meta: en 5 minutos el jugador ya entendió todo el loop, sin tutorial.

## Monedas
| Moneda | Se gana | Se gasta en | Se pierde al terminar |
|---|---|---|---|
| Oro | Matando enemigos | Torres y mejoras dentro de la partida | Sí |
| Gemas | Al perder: cada 5 olas + bonus | Taller (mejoras permanentes) | No |

## Plataforma y estilo
- **PC** primero.
- **3D low-poly**, con assets propios o CC0, legible con muchos objetos en pantalla.

## Fuera de alcance por ahora
- Mobile, multijugador, monetización.
- Modo idle completo: llega después de que el loop activo sea divertido.

## Preguntas abiertas
Se deciden en issues con la etiqueta `idea`:
- Temática y nombre del juego.
- Torres iniciales y cómo se desbloquean.
- Árbol del taller.
- Estilo visual.

## Registro de decisiones
| Fecha | Decisión |
|---|---|
| 2026-11-25 | Pasa de TP de tower defense a tower defense incremental. Del TP solo se reutiliza la base técnica. |
| 2026-11-25 | Inicio: 1 slot + primera partida que termina en un jefe imposible en la ola 5. |
| 2026-11-25 | Dos monedas: oro (en partida) y gemas (permanentes). |
| 2026-11-25 | 3D, PC primero; idle a largo plazo. |
