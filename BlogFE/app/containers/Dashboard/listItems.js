import React from 'react';
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';
import ListSubheader from '@material-ui/core/ListSubheader';
import DashboardIcon from '@material-ui/icons/Dashboard';
import ShoppingCartIcon from '@material-ui/icons/ShoppingCart';
import PeopleIcon from '@material-ui/icons/People';
import BarChartIcon from '@material-ui/icons/BarChart';
import LayersIcon from '@material-ui/icons/Layers';
import PostAddIcon from '@material-ui/icons/PostAdd';
import HomeIcon from '@material-ui/icons/Home';
import AssignmentIcon from '@material-ui/icons/Assignment';
import Link from '@material-ui/core/Link';

export const mainListItems = (
  <div>
    <ListItem button>
    <Link variant="subtitle1" href="/">

      <ListItemIcon>
        <HomeIcon />
      </ListItemIcon>
      </Link>

      <Link variant="subtitle1" href="/">
        <ListItemText primary="Back To Home Page" />
      </Link>
    </ListItem>
    <ListItem button>
      <ListItemIcon>
        <DashboardIcon />
      </ListItemIcon>
      <Link variant="subtitle1" href="/dashboard/manageposts">
        <ListItemText primary="Manage Posts" />
      </Link>
    </ListItem>
    <ListItem button>
      <ListItemIcon>
        <PostAddIcon />
      </ListItemIcon>
      <Link variant="subtitle1" href="/dashboard/createpost">
        <ListItemText primary="Create Post" />
      </Link>
    </ListItem>
  </div>
);

export const secondaryListItems = (
  <div>
    <ListSubheader inset>Profile</ListSubheader>
    <ListItem button>
      <ListItemIcon>
        <AssignmentIcon />
      </ListItemIcon>
      <ListItemText primary="Edit Your Profile" />
    </ListItem>
    <ListItem button>
      <ListItemIcon>
        <AssignmentIcon />
      </ListItemIcon>
      <ListItemText primary="Settings" />
    </ListItem>
  </div>
);
