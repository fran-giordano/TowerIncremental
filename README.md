# Tank Defense

Jogo 3D do tipo **Tower Defense** desenvolvido em **Unity 6** como trabalho prático da disciplina Computação Gráfica e Realidade Virtual (2026 - Prof. Rafael Rieder).

Defenda o castelo contra ondas de tanques inimigos posicionando torres militares com comportamentos diferentes.

## Download e vídeo
**Executável (Windows)** 
https://drive.google.com/file/d/1HpgQEIhXREX7ZOR0d-dZi9fy-gE5IOGz/view?usp=sharing

**Vídeo demonstrativo**
https://drive.google.com/file/d/1HaL5aeNBmT8Gmhhy1-oc1OpkOhd2zJy2/view?usp=sharing

Aviso: Podem ocorrer problemas com o Windows Defender, você pode escolher ignorar e executar mesmo assim

> Para jogar: baixe o `.zip`, extraia e execute `Tank Defense.exe`.

## Roadmap e contribuição
O projeto continua aberto, sem prazos. Ideias, código, arte e música são bem-vindos.
- **Roadmap:** aba *Projects* deste repositório (quadro Ideas → Para hacer → En curso → Hecho).
- **Propor algo:** abra uma *issue* com o modelo 💡 Idea.
- **Como contribuir:** ver [CONTRIBUTING.md](CONTRIBUTING.md).

## Equipe
- Luiza Strehl — [@llstrehl](https://github.com/llstrehl)
- Francisco Giordano — [@fran-giordano](https://github.com/fran-giordano)

## Como jogar
1. No menu principal, escolha **Jogar** e depois a dificuldade (**Fácil** ou **Difícil**).
2. Selecione uma torre na barra inferior (ou teclas **1–5**). Ao passar o mouse sobre um
    slot, um círculo mostra o **alcance** da torre: verde = pode construir, vermelho = slot ocupado ou moedas insuficientes.
3. **Clique esquerdo** no slot para construir. **Clique direito** cancela a seleção.
4. Cada inimigo eliminado dá moedas. Depois que todos os inimigos de uma onda saíram, 
    **N** (ou o botão *Próxima onda*) chama a próxima onda antes do tempo e dá **+10 moedas**.
5. **Esc** pausa o jogo.

**Vitória:** eliminar todas as ondas da dificuldade dentro do tempo limite.
**Derrota:** a vida do castelo chega a zero **ou** o tempo limite acaba.
 
**Pontuação:** `inimigos eliminados × 10`; em caso de vitória soma `segundos restantes × 10 + vida do castelo × 50`; o resultado é multiplicado pelo multiplicador da dificuldade (Fácil x1, Dificil x2)
O **ranking** guarda os 10 melhores resultados localmente e pode ser consultado no menu.

## Torres
 
| Torre | Modelo | Alcance | Dano | Intervalo | Custo | Comportamento |
|---|---|---|---|---|---|---|
| Básica | Metralhadora sobre caixa | 5 | 10 | 0,6 s | 50 | Ataca o inimigo **mais próximo** |
| Metralhadora | Arma rotativa em anel de sacos de areia | 4 | 5 | 0,2 s | 75 | Disparo muito rápido, dano baixo |
| Lenta | Estação de rádio (“inibidor”) | 5 | 2 | 1,0 s | 60 | Reduz a velocidade do inimigo em **50 % por 2 s** |
| Sniper | Torre de vigia | 10 | 40 | 2,5 s | 90 | Ataca o inimigo **mais adiantado no caminho**; projétil rápido |
| Explosiva | Morteiro em ninho de sacos de areia | 6 | 20 | 2,0 s | 120 | Dano **em área** (raio 3,5) |
 
Cada torre tem uma base fixa e uma **cabeça** (`Head`) que gira apenas no eixo Y em direção ao alvo;
o projétil sai do ponto `Muzzle`, na ponta do cano.
 
## Inimigos
 
| Inimigo | Velocidade | Vida | Dano à base | Recompensa |
|---|---|---|---|---|
| Normal | 3 | 50 | 1 | 10 |
| Rápido | 6 | 25 | 1 | 8 |
| Tanque | 1,5 | 200 | 3 | 25 |
 
Os inimigos seguem um caminho definido por *waypoints* até o castelo. Ao chegar, causam dano à base e são removidos.
 
## Dificuldades
 
| | Fácil | Difícil |
|---|---|---|
| Ondas | 3 | 5 |
| Inimigos | 19 | 57 |
| Vida do castelo | 10 | 7 |
| Moedas iniciais | 180 | 160 |
| Tempo limite | 130 s | 180 s |
| Intervalo entre inimigos | 1,4 s | 0,9 s |
| Pausa entre ondas | 8 s | 4 s |
 

## Arquitetura
 
O jogo é **orientado a dados**: torres, inimigos e dificuldades são `ScriptableObject`s (`Assets/Data`),
então um novo tipo pode ser criado ou balanceado sem escrever código.
 
```
Assets/
├─ Data/           TowerData, EnemyData e DifficultyData (valores do jogo)
├─ Prefabs/        Torres, inimigos, projéteis, slots, efeitos e UI
├─ Scenes/         MainMenu (menu) e projecOutdate (fase)
└─ Scripts/
   ├─ Core/        GameManager (estados, tempo, moedas, vitória/derrota), DifficultyData, ScoreBoard (ranking), FX, BaseZone
   ├─ Enemies/     Enemy (movimento por waypoints, slow), HealthTarget (vida/dano), WaveSpawner (ondas), WaypointPath
   ├─ Towers/      Tower (detecção, mira e disparo), Projectile (movimento e colisão), TowerPlacer (seleção e clique), TowerSlot
   ├─ UI/          HUD, TowerShopUI, MainMenu, PauseMenu, EndGamePanel, HealthBar
   └─ Art/         SimpleGifPlayer (animação de explosão), SpinInPlace
```

**Fluxo de uma partida**
 
1. `MainMenu` guarda a dificuldade escolhida e carrega a fase.
2. `GameManager` inicializa moedas, vida do castelo e tempo limite a partir da `DifficultyData`.
3. `WaveSpawner` cria os inimigos de cada onda; `Enemy` percorre os waypoints.
4. `TowerPlacer` lança um *raycast* do mouse para detectar o slot e instancia a torre escolhida.
5. `Tower` procura inimigos com `Physics.OverlapSphere`, gira a cabeça e instancia um `Projectile`.
6. `Projectile` se desloca até o alvo e, ao colidir (*trigger*), aplica dano, slow ou dano em área.
7. O jogo termina quando as ondas acabam (vitória), a vida do castelo zera ou o tempo esgota (derrota);
   a pontuação vai para o ranking.

## Conceitos de Computação Gráfica utilizados
 
| Conceito | Onde |
|---|---|
| Transformações geométricas e hierarquia | Cabeça das torres gira (rotação em Y) independente da base; escala por tipo de inimigo |
| Detecção de colisão | Projétil × inimigo (*triggers*), inimigo × base, dano em área com `OverlapSphere` |
| Raycast | Seleção de slots com o mouse |
| Criação e destruição de objetos | Torres, inimigos, projéteis e efeitos com `Instantiate`/`Destroy` |
| Temporização | Intervalo entre disparos, ondas e tempo limite com `Time.deltaTime`; pausa com `Time.timeScale` |
| Efeitos visuais e sonoros | Efeitos de disparo/impacto, animação de explosão, barras de vida, música e sons |
 
## Tecnologias
 
- Unity **6000.3.28f1** com Universal Render Pipeline (URP)
- C#, TextMeshPro, Input Manager

## Créditos

### Assets 3D
| Asset | Autor | Uso | Licença |
|---|---|---|---|
| [Military FREE – Low Poly 3D Models Pack](https://assetstore.unity.com/packages/3d/environments/military-free-low-poly-3d-models-pack-260358) | ithappy | Torres e decoração do cenário | Standard Unity Asset Store EULA (gratuito) |
| [Small Tank](https://assetstore.unity.com/packages/3d/vehicles/land/small-tank-186792) | Comeback | Modelo dos inimigos | Standard Unity Asset Store EULA (gratuito) |

### Música e efeitos sonoros
| Áudio | Autor / origem | Uso |
|---|---|---|
| MISATO’s Theme (EM07) | KenBan (YouTube) | Trilha |
| Spamton | Toby Fox | Trilha |
| Som de tiro | Em | Disparo das torres |
| Som de explosão | Roblox | Explosão |

> Os áudios pertencem aos seus respectivos autores e foram usados apenas para fins acadêmicos, sem fins lucrativos.

