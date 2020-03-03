/**
 *
 * HomePage
 *
 */

import React, { memo } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { Helmet } from 'react-helmet';
import { createStructuredSelector } from 'reselect';
import { compose } from 'redux';

import { useInjectSaga } from 'utils/injectSaga';
import { useInjectReducer } from 'utils/injectReducer';

import userManager from 'api/userManager';

import { makeStyles } from '@material-ui/core/styles';
import CssBaseline from '@material-ui/core/CssBaseline';
import Toolbar from '@material-ui/core/Toolbar';
import IconButton from '@material-ui/core/IconButton';
import SearchIcon from '@material-ui/icons/Search';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';
import Grid from '@material-ui/core/Grid';
import Card from '@material-ui/core/Card';
import CardActionArea from '@material-ui/core/CardActionArea';
import CardContent from '@material-ui/core/CardContent';
import CardMedia from '@material-ui/core/CardMedia';
import Hidden from '@material-ui/core/Hidden';
import Link from '@material-ui/core/Link';
import AppBar from '@material-ui/core/AppBar';
import Button from '@material-ui/core/Button';
import Divider from '@material-ui/core/Divider';
import Menu from '@material-ui/core/Menu';
import MenuItem from '@material-ui/core/MenuItem';
import AccountCircle from '@material-ui/icons/AccountCircle';
import Container from '@material-ui/core/Container';
import useScrollTrigger from '@material-ui/core/useScrollTrigger';
import Slide from '@material-ui/core/Slide';
import ListPosts from 'components/ListPosts';
import Copyright from 'components/Copyright';
import { makeSelectOidc, makeSelectCategories } from 'containers/App/selectors';
import makeSelectHomePage from './selectors';
import reducer from './reducer';
import saga from './saga';
import { getListPostViewRequest, resetPage } from './actions';

const useStyles = makeStyles(theme => ({
  appBar: {
    backgroundColor: '#fff',
    color: 'black',
  },
  toolbar: {
    borderBottom: `1px solid ${theme.palette.divider}`,
  },
  toolbarTitle: {
    flex: 1,
  },
  toolbarSecondary: {
    justifyContent: 'space-between',
    overflowX: 'auto',
  },
  toolbarLink: {
    padding: theme.spacing(1),
    flexShrink: 0,
  },
  mainFeaturedPost: {
    position: 'relative',
    backgroundColor: theme.palette.grey[800],
    color: theme.palette.common.white,
    marginBottom: theme.spacing(4),
    backgroundImage: 'url(https://source.unsplash.com/user/erondu)',
    backgroundSize: 'cover',
    backgroundRepeat: 'no-repeat',
    backgroundPosition: 'center',
  },
  overlay: {
    position: 'absolute',
    top: 0,
    bottom: 0,
    right: 0,
    left: 0,
    backgroundColor: 'rgba(0,0,0,.3)',
  },
  mainFeaturedPostContent: {
    position: 'relative',
    padding: theme.spacing(3),
    [theme.breakpoints.up('md')]: {
      padding: theme.spacing(6),
      paddingRight: 0,
    },
  },
  mainGrid: {
    marginTop: theme.spacing(3),
  },
  card: {
    display: 'flex',
  },
  cardDetails: {
    flex: 1,
  },
  cardMedia: {
    width: 160,
  },
  markdown: {
    ...theme.typography.body2,
    padding: theme.spacing(3, 0),
  },
  sidebarAboutBox: {
    padding: theme.spacing(2),
    backgroundColor: theme.palette.grey[200],
  },
  sidebarSection: {
    marginTop: theme.spacing(3),
  },
  footer: {
    backgroundColor: theme.palette.background.paper,
    marginTop: theme.spacing(8),
    padding: theme.spacing(6, 0),
  },
}));

export function HomePage({
  location,
  match,
  homePage,
  dispatch,
  history,
  oidc,
  categories,
}) {
  useInjectReducer({ key: 'homePage', reducer });
  useInjectSaga({ key: 'homePage', saga });

  const { listPostView } = homePage;
  const { user } = oidc;
  const trigger = useScrollTrigger();

  React.useEffect(() => {
    // match.params.name
    dispatch(getListPostViewRequest('homePage', 1));
    return () => {
      dispatch(resetPage());
    };
  }, []);

  const classes = useStyles();

  const logout = () => {
    userManager.signoutRedirect();
    userManager.removeUser();
  };

  const login = () => {
    userManager.signinRedirect({
      data: { path: location.pathname },
    });
  };

  const loadPost = page => {
    dispatch(getListPostViewRequest('homePage', page));
  };

  const [anchorEl, setAnchorEl] = React.useState(null);
  const open = Boolean(anchorEl);

  const handleMenu = event => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  return (
    <React.Fragment>
      <CssBaseline />
      <Container maxWidth="lg">
        <Slide appear={false} direction="down" in={!trigger}>
          <AppBar className={classes.appBar} color="inherit" position="fixed">
            <Toolbar className={classes.toolbar}>
              <Typography
                component="h2"
                variant="h5"
                color="inherit"
                align="center"
                noWrap
                className={classes.toolbarTitle}
                onClick={() => history.push('/')}
              >
                Tech Blog
              </Typography>
              {!user && (
                <Button variant="outlined" size="small" onClick={login}>
                  Login
                </Button>
              )}
              {user && (
                <div>
                  <IconButton
                    aria-label="account of current user"
                    aria-controls="menu-appbar"
                    aria-haspopup="true"
                    onClick={handleMenu}
                    color="inherit"
                  >
                    <AccountCircle />
                  </IconButton>
                  <Menu
                    id="menu-appbar"
                    anchorEl={anchorEl}
                    anchorOrigin={{
                      vertical: 'top',
                      horizontal: 'right',
                    }}
                    keepMounted
                    transformOrigin={{
                      vertical: 'top',
                      horizontal: 'right',
                    }}
                    open={open}
                    onClose={handleClose}
                  >
                    <MenuItem
                      onClick={() => history.push('/dashboard/manageposts')}
                    >
                      Dashboard
                    </MenuItem>
                    <MenuItem onClick={logout}>Logout</MenuItem>
                  </Menu>
                </div>
              )}
            </Toolbar>
            <Toolbar
              component="nav"
              variant="dense"
              className={classes.toolbarSecondary}
            >
              {categories &&
                categories.map(section => (
                  <Link
                    color="inherit"
                    noWrap
                    key={section.name}
                    variant="body2"
                    href={`/category/${section.name}`}
                    className={classes.toolbarLink}
                  >
                    {section.name}
                  </Link>
                ))}
            </Toolbar>
          </AppBar>
        </Slide>
        <Toolbar />
        <Toolbar />
        <main>
          <Grid container spacing={4}>
            <Grid item key={new Date().valueOf()} xs={12} md={8}>
              {listPostView && listPostView.posts && (
                <ListPosts
                  posts={listPostView.posts}
                  loadPost={loadPost}
                  hasMore={listPostView.hasMore}
                />
              )}
            </Grid>
            <Grid item xs={12} md={4}>
              <Copyright />
            </Grid>
          </Grid>
        </main>
      </Container>
    </React.Fragment>
  );
}

HomePage.propTypes = {
  dispatch: PropTypes.func.isRequired,
  location: PropTypes.object,
  match: PropTypes.object,
  homePage: PropTypes.object,
  history: PropTypes.any,
  oidc: PropTypes.any,
  categories: PropTypes.array,
};

const mapStateToProps = createStructuredSelector({
  homePage: makeSelectHomePage(),
  oidc: makeSelectOidc(),
  categories: makeSelectCategories(),
});

function mapDispatchToProps(dispatch) {
  return {
    dispatch,
  };
}

const withConnect = connect(
  mapStateToProps,
  mapDispatchToProps,
);

export default compose(
  withConnect,
  memo,
)(HomePage);
