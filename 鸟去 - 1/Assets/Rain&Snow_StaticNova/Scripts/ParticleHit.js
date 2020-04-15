
function LateUpdate () 
{
	var theParticles = GetComponent.<ParticleEmitter>().particles;
	print("Particle" + 1 + "energy is" + theParticles[0].energy);
	
	if(theParticles[0].energy > GetComponent.<ParticleEmitter>().maxEnergy)
	{
		print("Hit Surface.");
	}
}