using System.Collections.Generic;

public class mario_yaoshi : mario_charater
{
	public override void init(string name, List<int> param, int world, int x, int y, int xx, int yy)
	{
		base.init(name, param, world, x, y, xx, yy);
		m_is_guaiwu = false;
	}

	public override void be_hit(mario_obj obj)
	{
		if (obj.m_main)
		{
			m_is_destory = 1;
			play_mode._instance.caisi(0);
			mario._instance.play_sound("sound/get");
			play_mode._instance.add_score(m_pos.x, m_pos.y, 1000);
			play_mode._instance.m_ys++;
		}
	}
}
