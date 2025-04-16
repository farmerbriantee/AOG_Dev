enum PGNs {
	AgIOHello = 0xC8,
	SubnetChange = 0xC9,
	SubnetRequest = 0xCA,
	ToolSteerConfig = 0xE7,
	ToolSteerSettings = 0xE8,
	ToolSteer = 0xE9,
	MachineSettings = 0xEE,
	MachineData = 0xEF,
	SteerConfig = 0xFB,
	SteerSettings = 0xFC,
	SteerData = 0xFE
};


enum toolIDs {
	xteLo = 5,
	xteHi = 6,
	status = 7,
	xteVehLo = 8,
	xteVehHi = 9,
	speed10 = 10,
	headLo = 11,
	headHi = 12,
};

enum toolSteerIDs {
	 gainP = 5,
	 integral = 6,
	 minPWM = 7,
	 highPWM = 8,
	 countsPerDegree = 9,
	 wasOffsetLo = 10,
	 wasOffsetHi = 11,
	 ackerman = 12,
};

enum toolSteerConfig {
	 invertWAS = 5,
	 invertSteer = 6,
	 maxSteerAngle = 7,
	 isSteer = 8,
};