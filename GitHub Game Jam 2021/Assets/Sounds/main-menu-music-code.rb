vibe = "/programming-stuff/game-jam/vibe-pack/"
loop do
  sample vibe, [15, 18, 22, 25].choose, pan: rrand(-0.75, 0.75), amp: rrand(0.25, 1)
  sleep 0.1
end