# Visualizador de Pathfinding 2D – UNIVALI

Este projeto foi desenvolvido para fins didáticos, a fim de demonstrar o funcionamento dos principais algoritmos de **busca de caminho (Pathfinding)** em jogos e sistemas interativos.  
O projeto foi criado na **Unity**, utilizando **C#**, e inclui um sistema de visualização passo a passo dos algoritmos **BFS**, **Dijkstra** e **A\*** em um ambiente **2D baseado em grid**.

---

## Objetivo do Projeto

O propósito deste projeto é permitir que os alunos do curso de **Design de Jogos da UNIVALI** visualizem na prática como os algoritmos de pathfinding funcionam.  
Com ele, é possível observar o comportamento de cada algoritmo em tempo real, compreender seus custos de processamento e diferenças conceituais.

---

## O que é Pathfinding?

**Pathfinding** é o processo de encontrar um caminho entre dois pontos (início e fim) em um ambiente composto por múltiplos nós conectados.  
Em jogos, pathfinding é amplamente utilizado para movimentação de personagens, inimigos, NPCs ou até mesmo sistemas de IA em estratégias, simulações e navegação.

O objetivo de um algoritmo de pathfinding é encontrar **o melhor caminho possível** entre dois pontos, levando em conta **distância, custo de movimento e obstáculos**.

---

## Algoritmos Implementados

### BFS (Breadth-First Search)
- O algoritmo mais simples entre os três.
- Explora todos os nós vizinhos de forma uniforme, **sem considerar custos**.
- Ideal para grids onde todos os movimentos têm o mesmo custo.
- **Desvantagem:** ineficiente em ambientes grandes ou com custos variáveis.

### Dijkstra
- Baseado em um **sistema de custos acumulados (G-cost)**.
- Sempre escolhe o próximo nó com o **menor custo total até o momento**.
- Garante o **caminho mais curto possível**, mesmo em terrenos com diferentes custos.
- **Desvantagem:** não utiliza heurística, portanto pode explorar muitos nós desnecessários.

### A* (A-Star)
- Extensão do Dijkstra que adiciona uma **heurística (H-cost)**.
- O cálculo total é dado por **F = G + H**, onde:
  - **G** é o custo real percorrido.
  - **H** é a estimativa de distância até o destino.
- Permite equilibrar velocidade e precisão.
- **Mais eficiente** que Dijkstra em muitos casos, sendo amplamente utilizado em jogos modernos.

---

## Como utilizar

### 1. Configuração do Projeto
- Abra o projeto na **Unity (versão 2021.3 ou superior)**.
- Certifique-se de que os scripts `GridPathfinding2D.cs` e `Node2D.cs` estão corretamente adicionados a um **GameObject vazio** na cena (ex: “PathfindingManager”).
- O prefab **Node2D** deve conter:
  - Um `SpriteRenderer` (para o círculo do nó)
  - Um `TextMeshPro` para exibir valores G, H, F e Custo

### 2. Execução
- Pressione **Play** na Unity.
- O grid será automaticamente gerado com base nos parâmetros definidos no inspetor:
  - **Grid Width** e **Grid Height** – tamanho da grade
  - **Node Spacing** – espaçamento entre nós
  - **Step Delay** – tempo entre etapas da animação
  - **Heuristic Weight** – influência da heurística no A*

### 3. Controles
| Ação | Função |
|------|---------|
| Clique esquerdo | Define o nó inicial |
| Clique direito | Define o nó final |
| Scroll do mouse | Aumenta ou diminui o custo do nó (0–10) |
| Barra de espaço | Executa o algoritmo selecionado |
| Delete | Reseta as cores dos nós |
| Enum “Search Mode” no Inspetor | Alterna entre A*, Dijkstra e BFS |

### 4. Cores e Significados
| Cor | Significado |
|------|-------------|
| Branco | Nó não visitado |
| Ciano | Nó na lista de abertos (descoberto) |
| Magenta | Nó na lista de fechados (já visitado) |
| Amarelo | Caminho final encontrado |
| Verde | Nó inicial |
| Vermelho | Nó final |
| Escurecimento | Indica o custo do nó (quanto mais escuro, maior o custo) |

---

## Estrutura de Scripts

### GridPathfinding2D.cs
- Gera a grade de nós (em forma de grid quadrado).
- Controla a execução dos algoritmos.
- Contém as três corrotinas: `RunBFS()`, `RunDijkstra()` e `RunAStar()`.
- Permite visualizar passo a passo o processo de busca.

### Node2D.cs
- Representa cada nó do grid.
- Armazena custos G, H, F e custo adicional.
- Atualiza as cores e rótulos automaticamente.
- Permite interação por clique e scroll.

---

## Possíveis Extensões
Este projeto serve de base para diversas expansões:
- Implementação de movimentação em **hexágonos**.
- Adição de **diagonais** no grid.
- Criação de **obstáculos** clicáveis.
- Comparação direta de performance entre algoritmos.
- Exibição gráfica dos nós em tempo real via **UI**.

---

## Créditos e Licença

Este projeto foi desenvolvido por **Eduardo Poffo Medeiros Dias** para o curso de **Design de Jogos da UNIVALI**, com fins exclusivamente educacionais.

O código pode ser livremente utilizado e adaptado para estudos e projetos acadêmicos, desde que os devidos créditos sejam mantidos.

---
