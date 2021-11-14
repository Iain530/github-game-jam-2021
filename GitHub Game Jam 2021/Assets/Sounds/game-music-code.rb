use_bpm 95
#Song is in Gbm
#vibe has notes 0-35

bass = "/programming-stuff/game-jam/double-bass-pack/"
vibe = "/programming-stuff/game-jam/vibe-pack/"

#bass notes:
# A 0-3
#C 1-4
#Eb 1-3
#Gb 1-3

snare = :drum_snare_soft
cymbal = :drum_cymbal_soft
snarePan = 0.15
snareAmp = 0.8
cymbalPan = -0.3
cymbalAmp = 0.3

in_thread do
  #2 bar loop
  2.times do
    3.times do
      sample :drum_bass_soft
      sample cymbal, pan: cymbalPan, amp: cymbalAmp
      sleep 1
      sample snare, pan: snarePan, amp: snareAmp
      sample cymbal, pan: cymbalPan, amp: cymbalAmp
      sleep 0.667
      sample cymbal, pan: cymbalPan, amp: cymbalAmp
      sleep 0.333
    end
    sample :drum_bass_soft
    sample cymbal, pan: cymbalPan, amp: cymbalAmp
    sleep 0.667
    sample snare, pan: snarePan, amp: snareAmp
    sleep 0.333
    sample cymbal, pan: cymbalPan, amp: cymbalAmp
    sleep 0.667
    sample cymbal, pan: cymbalPan, amp: cymbalAmp
    sleep 0.333
  end
  3.times do
    sample :drum_bass_soft
    sample cymbal, pan: cymbalPan, amp: cymbalAmp
    sleep 1
    sample snare, pan: snarePan, amp: snareAmp
    sample cymbal, pan: cymbalPan, amp: cymbalAmp
    sleep 0.667
    sample cymbal, pan: cymbalPan, amp: cymbalAmp
    sleep 0.333
  end
  sample :drum_bass_soft
  sample cymbal, pan: cymbalPan, amp: cymbalAmp
  sleep 1
  sample cymbal, pan: cymbalPan, amp: cymbalAmp
  sleep 0.667
  sample cymbal, pan: cymbalPan, amp: cymbalAmp
  sleep 0.333
  
  loop do
    3.times do
      sample :drum_bass_soft
      sample cymbal, pan: cymbalPan, amp: cymbalAmp
      sleep 1
      sample snare, pan: snarePan, amp: snareAmp
      sample cymbal, pan: cymbalPan, amp: cymbalAmp
      sleep 0.667
      sample cymbal, pan: cymbalPan, amp: cymbalAmp
      sleep 0.333
    end
    sample :drum_bass_soft
    sample cymbal, pan: cymbalPan, amp: cymbalAmp
    sleep 0.667
    sample snare, pan: snarePan, amp: snareAmp
    sleep 0.333
    sample cymbal, pan: cymbalPan, amp: cymbalAmp
    sleep 0.667
    sample cymbal, pan: cymbalPan, amp: cymbalAmp
    sleep 0.333
  end
end

in_thread do
  sync :snare
  sleep 0.334
  sample snare, pan: snarePan, amp: 0.4
  sleep 0.333
  sample snare, pan: snarePan, amp: 0.4
  sleep 0.333
  sample snare, pan: snarePan, amp: 0.6
  sleep 0.334
  sample snare, pan: snarePan, amp: 0.4
  sleep 0.333
  sample snare, pan: snarePan, amp: 0.4
end


in_thread do
  loop do
    #bar 1
    sample bass, 3, sustain: 1
    sleep 1
    sample bass, 3, rpitch: 2, sustain: 1
    sleep 1
    sample bass, 4, sustain: 1
    sleep 1
    sample bass, 3, rpitch: 2, sustain: 1
    sleep 1
    #bar 2
    sample bass, 4, sustain: 1
    sleep 1
    sample bass, 3, rpitch: 1, sustain: 1
    sleep 1
    sample bass, 3, sustain: 1
    sleep 1
    sample bass, 3, rpitch: 1, sustain: 1
    sleep 1
    #bar 3
    cue :vibra1
    sample bass, 3, sustain: 1
    sleep 1
    sample bass, 3, rpitch: 2, sustain: 1
    sleep 1
    sample bass, 4, sustain: 1
    sleep 1
    sample bass, 4, rpitch: 2, sustain: 1
    sleep 1
    #bar 4
    sample bass, 5, rpitch: 1, sustain: 1
    sleep 1
    sample bass, 4, rpitch: 2, sustain: 1
    sleep 1
    sample bass, 4, sustain: 1
    sleep 1
    sample bass, 4, rpitch: 2, sustain: 1
    sleep 1
    #bar 5
    sample bass, 3, sustain: 1
    sleep 1
    sample bass, 3, rpitch: 2, sustain: 1
    sleep 1
    sample bass, 4, sustain: 1
    sleep 1
    sample bass, 3, rpitch: 2, sustain: 1
    sleep 1
    #bar 6
    sample bass, 4, sustain: 1
    sleep 1
    sample bass, 3, rpitch: 1, sustain: 1
    sleep 1
    cue :snare
    sample bass, 3, sustain: 1
    sleep 1
    sample bass, 3, rpitch: 1, sustain: 1
    sleep 1
    #bar 7
    sample bass, 3, sustain: 1
    sleep 1
    sample bass, 2, rpitch: 1, sustain: 1
    sleep 1
    sample bass, 1, rpitch: 2, sustain: 1
    sleep 1
    sample bass, 1, rpitch: 1, sustain: 1
    sleep 1
    #bar 8
    sample bass, 1, rpitch: 2, sustain: 1
    sleep 1
    sample bass, 2, rpitch: 2, sustain: 1
    sleep 1
    sample bass, 3, rpitch: 2, sustain: 1
    sleep 1
    sample bass, 4, rpitch: 2, sustain: 1
    sleep 1
    #bar 9
    sample bass, 3, sustain: 1
    sleep 1
    sample bass, 4, sustain: 1
    sleep 1
    sample bass, 5, sustain: 1
    sleep 1
    sample bass, 5, rpitch: 1, sustain: 1
    sleep 1
    #bar 10
    sample bass, 5, rpitch: 2, sustain: 1
    sleep 1
    sample bass, 5, sustain: 1
    sleep 1
    sample bass, 5, rpitch: 1, sustain: 1
    sleep 1
    sample bass, 4, sustain: 1
    sleep 1
    #bar 11
    sample bass, 5, rpitch: 1, sustain: 1
    sleep 1
    sample bass, 5, rpitch: 1, sustain: 1
    sleep 1
    sample bass, 5, rpitch: 1, sustain: 1
    sleep 1
    sample bass, 5, rpitch: 1, sustain: 0.667
    sleep 0.667
    sample bass, 4, rpitch: 2, sustain: 0.333
    sleep 0.333
    #bar 12
    cue :vibra2
    sample bass, 5, rpitch: 1, sustain: 1
    sleep 1
    sample bass, 5, rpitch: 1, sustain: 1
    sleep 1
    sample bass, 5, rpitch: 1, sustain: 1
    sleep 1
    sample bass, 4, rpitch: 2, sustain: 0.667
    sleep 0.667
    sample bass, 4, sustain: 0.333
    sleep 0.333
  end
end

in_thread do
  sync :vibra1
  #bar 3
  sample vibe, 25
  sample vibe, 28
  sample vibe, 32
  sample vibe, 35
  sleep 3.667
  sample vibe, 23
  sample vibe, 27
  sample vibe, 30
  sample vibe, 33
  sleep 0.333
  #bar 4
  sleep 2.667
  sample vibe, 25
  sleep 0.333
  sample vibe, 23
  sleep 0.333
  sample vibe, 18
  sleep 0.333
  sample vibe, 20
  sleep 0.334
  #bar 5
  sample vibe, 21
  sleep 1.667
  sample vibe, 21
  sample vibe, 25
  sample vibe, 28
  sleep 2
  sample vibe, 23
  sample vibe, 27
  sample vibe, 33
  sleep 0.333
  #bar 6
  sleep 4
  #bar 7
  sleep 4
  #bar 8
  sleep 4
  #bar 9
  sleep 2
end

in_thread do
  sync :vibra2
  sample vibe, 22
  sleep 0.167
  sample vibe, 23
  sleep 0.166
  sample vibe, 24
  sleep 0.166
  sample vibe, 25
  sleep 0.167
  sample vibe, 26
  sleep 0.166
  sample vibe, 27
  sleep 0.166
  sample vibe, 28
end
