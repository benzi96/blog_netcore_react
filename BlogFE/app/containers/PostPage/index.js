/**
 *
 * PostPage
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
import AppBar from '@material-ui/core/AppBar';
import IconButton from '@material-ui/core/IconButton';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';
import Grid from '@material-ui/core/Grid';
import Card from '@material-ui/core/Card';
import Link from '@material-ui/core/Link';
import Button from '@material-ui/core/Button';
import Divider from '@material-ui/core/Divider';
import Container from '@material-ui/core/Container';
import Copyright from 'components/Copyright';
import useScrollTrigger from '@material-ui/core/useScrollTrigger';
import Slide from '@material-ui/core/Slide';
import Breadcrumbs from '@material-ui/core/Breadcrumbs';
import Menu from '@material-ui/core/Menu';
import MenuItem from '@material-ui/core/MenuItem';
import AccountCircle from '@material-ui/icons/AccountCircle';
import { Editor, EditorState, convertFromRaw } from 'draft-js';
import { makeSelectOidc, makeSelectCategories } from 'containers/App/selectors';

// import Markdown from './Markdown';
import makeSelectPostPage from './selectors';
import reducer from './reducer';
import saga from './saga';
import { loadPostRequest, resetPage } from './actions';

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
    cursor: 'pointer',
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
  paper: {
    padding: '20px',
    minHeight: '500px',
  },
}));

export function PostPage({
  location,
  match,
  postPage,
  dispatch,
  history,
  oidc,
  categories,
}) {
  useInjectReducer({ key: 'postPage', reducer });
  useInjectSaga({ key: 'postPage', saga });
  React.useEffect(() => {
    dispatch(loadPostRequest(match.params.id));
    return () => {
      dispatch(resetPage());
    };
  }, []);

  const { post } = postPage;
  const { user } = oidc;

  const classes = useStyles();
  const login = () => {
    userManager.signinRedirect({
      data: { path: location.pathname },
    });
  };

  const trigger = useScrollTrigger();
  const [anchorEl, setAnchorEl] = React.useState(null);
  const open = Boolean(anchorEl);

  const handleMenu = event => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const logout = () => {
    userManager.signoutRedirect();
    userManager.removeUser();
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
          {post && (
            <Grid container spacing={5} className={classes.mainGrid}>
              {/* Main content */}
              <Grid item xs={12} md={8}>
                <Paper className={classes.paper}>
                  <Breadcrumbs aria-label="breadcrumb">
                    <Link color="inherit" href="/">
                      Tech Blog
                    </Link>
                    <Link
                      color="inherit"
                      href={`/category/${post.categoryName}`}
                    >
                      {post.categoryName}
                    </Link>
                  </Breadcrumbs>
                  <Divider />
                  <Typography variant="h3" gutterBottom>
                    {post.title}
                  </Typography>
                  <Typography variant="caption" gutterBottom>
                    {post.createDate}
                  </Typography>
                  <Divider />
                  <Typography variant="body1" gutterBottom>
                    {post.shortDescription}
                  </Typography>
                  <Editor editorState={post.content} readOnly />
                </Paper>
              </Grid>
              {/* End main content */}
              {/* Sidebar */}
              <Grid item xs={12} md={4}>
                <Copyright />
              </Grid>
              {/* End sidebar */}
            </Grid>
          )}
        </main>
      </Container>
    </React.Fragment>
  );
}

PostPage.propTypes = {
  dispatch: PropTypes.func.isRequired,
  location: PropTypes.any,
  match: PropTypes.any,
  postPage: PropTypes.object,
  history: PropTypes.any,
  oidc: PropTypes.any,
  categories: PropTypes.array,
};

const mapStateToProps = createStructuredSelector({
  postPage: makeSelectPostPage(),
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
)(PostPage);
